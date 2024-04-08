using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;





namespace BookSyncWorkerService
{
    public class BookService : BackgroundService
    {

        private readonly ILogger<BookService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string API_KEY = " "; // !! This must be extracted into env variable
        private readonly string OPENAI_API_KEY = " "; // !! This must be extracted into env variable
        private readonly IServiceScopeFactory _scopeFactory;


        public BookService(ILogger<BookService> logger, IHttpClientFactory httpClientFactory, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            /* int count = 0;
             while (!stoppingToken.IsCancellationRequested)
             {
                 *//*if (_logger.IsEnabled(LogLevel.Information))
                 {
                     _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                 }*//**/
                  
                  try {
                        await GetBooksAsync(stoppingToken);
                     }catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                 /*await Task.Delay(1000, stoppingToken);
             }*/


        }

        // Get books from the google books API
        private async Task GetBooksAsync(CancellationToken cancellationToken)
        {
            var clientRequest = _httpClientFactory.CreateClient("Book_API");
            // Create response array for each category of books 
            HttpResponseMessage[] response = new HttpResponseMessage[12];
            string[] requestByCategory = {"parenting", "economics", "science", "entrepreneurship", "psychology", "money", "health", "marriage", "exercise", "biography", "history", "politics" };

            // This filters fields I want to return from the API
            string fields = "items(volumeInfo/title,volumeInfo/authors,volumeInfo/imageLinks,volumeInfo/description,volumeInfo/industryIdentifiers,volumeInfo/categories)";
            // perform requests for each category
            /* Console.WriteLine($"LENGHT OF JSON RESULTS {}");*/

            for (int i = 0; i < requestByCategory.Length; i++)
            {
                response[i] = await clientRequest.GetAsync($"volumes?q=subject:{requestByCategory[i]}&maxResults=10&fields={fields}&key={API_KEY}", cancellationToken);

                if (response[i].IsSuccessStatusCode)
                { 
                    // Read the response content
                    string responseBody = await response[i].Content.ReadAsStringAsync();

                    // Process the response data as needed
                    Console.WriteLine($"My Request response: {requestByCategory[i]}");

                    var res = JObject.Parse(responseBody);
                    var bookData = res["items"];

                    // Prepare book data to be stored in database 
                    if (bookData != null)
                    {
                        foreach (var item in bookData)
                        {
                            var volumeInfo = item["volumeInfo"];
                            var title = volumeInfo["title"] != null ? volumeInfo["title"]?.ToString() : "Unknown Title";
                            var authors = volumeInfo["authors"];
                            var description = volumeInfo["description"] != null ? volumeInfo["description"]?.ToString(): "Description not available";
                            string thumbnailUrl = volumeInfo["imageLinks"]?["thumbnail"]?.ToString();

                            // Extract authors and concatenate them into a single string
                            string authorString = authors != null ? string.Join(", ", authors.Select(a => a.ToString())) : "Unknown Author";

                            // Check if ISBN is not null and extract it
                            string ISBN = null;
                            var industryIdentifiers = volumeInfo["industryIdentifiers"];
                            if (industryIdentifiers != null)
                            {
                                List<string> ISBNList = new List<string>();
                                for (int j = 0; j < industryIdentifiers.Count(); j++)
                                {
                                    var identifier = industryIdentifiers[j];
                                    if (identifier["identifier"] != null)
                                    {
                                        ISBNList.Add(identifier["identifier"].ToString());
                                    }
                                }
                                ISBN = string.Join("", ISBNList);
                            }
                            else
                            {
                                // generate a new "ISBN" to operate as a Primary Key
                                // In case ISBN/identifier is null

                            }

                            // preparing bookInfo for storage in the database
                            int shelf = i;
                            BookDetails newBook = new BookDetails(title, authorString, description, ISBN, thumbnailUrl,shelf);
                            newBook.Genre = requestByCategory[i];



                            // Check if book is already present in Database
                            
                            if (await BookExistsAsync(newBook.ISBN))
                            {
                                // book already exists
                                Console.WriteLine("Book already exists skipping.... insertion");
                                continue;
                            }

                            Console.WriteLine("Inserting book....");
                           

                            // =================== Summarize book using Open AI =====================

                            var openAIClient = _httpClientFactory.CreateClient("Open_AI_API");
                            await SummarizeDataAsync(openAIClient, newBook, cancellationToken);

                            //=================== Store book to database ========================
                            await StoreBookInfoAsync(newBook);

                        }
                    }
                }
                else
                {
                    // Handle unsuccessful response
                    Console.WriteLine($"Error making request: Status code: {response[i].StatusCode}, Reason: {response[i].ReasonPhrase}");
                    string errorContent = await response[i].Content.ReadAsStringAsync();
                    if (!string.IsNullOrWhiteSpace(errorContent))
                    {
                        Console.WriteLine("Error in Response Body:");
                        Console.WriteLine(errorContent);
                    }

                }
            }
        }

        private async Task StoreBookInfoAsync(BookDetails book)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Custom_Book_EndPoint");

                // Serialize the book object to JSON
                var newBook = new BookDetails
                {
                    Id = Guid.NewGuid(),
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Author = book.Author,
                    Description = book.Description,
                    Genre = book.Genre,
                    ImageUrl = book.ImageUrl,
                    Content = book.Content,
                    Quantity = book.Quantity,
                    Shelf = book.Shelf,
                    AccessFrequency = book.AccessFrequency
                };


                // Convert  the Book object to JSON
                var json = JsonConvert.SerializeObject(newBook);

                // prepare the request conten+  t
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send the POST request to store the book 
                var response = await client.PostAsync("Book/Create", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Book stored successfully.");
                }
                else
                {
                    Console.WriteLine($"Failed to store book info. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while storing book info: {ex.Message}");
            }
        }


        // Check if Book already exists in the database
        private async Task<bool> BookExistsAsync(string ISBN)
        {
            try
            {
                var clientRequest = _httpClientFactory.CreateClient("Custom_Book_EndPoint");
                var response = await clientRequest.GetAsync($"Book/GetBookByISBN?isbn={ISBN}"); // !!make sure it is the correct end-point
                var data = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<BookDetails>(data); 
                Console.WriteLine(response);
                 
                // Check if the request was successful
                if (responseData?.ISBN != null)
                {
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while checking if the book exists: {ex.Message}");
                return false;
            }

            return true; // It will never be reached. This is purely for syntax purposes

        }


        //  ====================================== AI Summarize Data =====================================================
        private async Task SummarizeDataAsync(HttpClient openAIClient, BookDetails book, CancellationToken cancellationToken)
        {
            // construct prompt to gpt
            var requestContent = new
            {
                messages = new[]
                {
                    new { role = "user", content = $"Please provide a summary of key ideas of the book with title {book.Title} by author(s) {book.Author} such that one can get value and read in less than 20 minutes" }
                },
                max_tokens = 800,
                model = "gpt-3.5-turbo"
            };

            HttpResponseMessage response = null;

            // Handle requests limits
            try
            {
                // Set up the authorization header with  OpenAI API key if it does not exist
                if (!openAIClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    openAIClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {OPENAI_API_KEY}");
                }
                // Send the request to the OpenAI API endpoint for text  summarization
                response = await openAIClient.PostAsJsonAsync("v1/chat/completions", requestContent, cancellationToken);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP request exception: {ex.Message}");
            }

            if (response != null && response.IsSuccessStatusCode)
            {
                // Read the response content
                string responseBody = await response.Content.ReadAsStringAsync();

                // Process the response data as needed

                // Format  summarized content 
                var jsonResponse = JObject.Parse(responseBody);
                string content = jsonResponse["choices"][0]["message"]["content"] != null ?
                                 jsonResponse["choices"][0]["message"]["content"].ToString() : "No summary available yet";

                         // ==================== Add content to the book ===============
                book.Content = content;

                //Console.WriteLine($"Testing summarization:::::::::: {content}");

            }
            else if (response != null && response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                // Handle rate limit exceeded error
                Console.WriteLine($"Rate limit exceeded. Waiting for cooldown period...");

                // Wait for the cooldown period of 60 seconds
                await Task.Delay(TimeSpan.FromSeconds(60));

                // Retry the request
                await SummarizeDataAsync(openAIClient, book, cancellationToken);
            }
            else
            {
                Console.WriteLine("Error making a request to summarize data response is null");
                return;
            }
        }
    }
}
