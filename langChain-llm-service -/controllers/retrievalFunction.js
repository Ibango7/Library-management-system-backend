// langchain llm
import { OpenAIEmbeddings } from '@langchain/openai';
import { MemoryVectorStore } from 'langchain/vectorstores/memory';
import { createRetrievalChain } from "langchain/chains/retrieval";
import { ChatPromptTemplate } from "@langchain/core/prompts";
import { ChatOpenAI } from "@langchain/openai";
import { createStuffDocumentsChain } from "langchain/chains/combine_documents";
import { getBooks } from '../repository/getDBbooks.js';
import dotenv from 'dotenv';
dotenv.config();

const apiKey = process.env.openAI_ApiKey;
// Check if the API key is defined
if (!apiKey) {
    throw new Error("OpenAI API key not found, error!");
}


// use gpt llm
const chatModel = new ChatOpenAI({
    openAIApiKey: apiKey,
});

const prompt = ChatPromptTemplate.fromTemplate(` You are an expert in recommending or suggesting books.
  Answer the following question as best as you can based only on the provided context. The genre or category of the context must be considered. Do not answer based on the general knowledge you have.
    <context>
    {context}
    </context>
    
    Question: {input}`);


export const retrievalFunction = async (req, res) => {

    // get user input
    const userInput = req.query.prompt;
    if(!userInput){
        return;
    }
    // getbooks from the database
    try {
        let vectorstore = null;
        let response = await getBooks()
        // console.log("books retrieed", response.result.items);
        // return book Data as Json object
        response = response.result.items;
        if (response) {

            let documents = [];
            let documentMetaData = [];
            // loop through book to embed information 
            for (let book of response) {
                // 1 - get response data and  concatenate it
                let title = book.title;
                let genre = book.genre;
                let description = book.description;
                let Author = book.author;
                let isbn = book.isbn;
                documents.push(title + "\n" + description + "\n" + genre + "`\n" + isbn);
                documentMetaData.push({ isbn: isbn })
                // console.log("title:::", title);
                // console.log("Book:::", description);
                // console.log("Author:::", Author);
                // console.log("isb:::", isbn);

            }

            // 2 - Use embedding model and store in Vector database

            vectorstore = await MemoryVectorStore.fromTexts(documents, documentMetaData, new OpenAIEmbeddings({
                openAIApiKey: apiKey,
            }));


            // 3 - Using llm for chaining
            // prompt gpt to search/recommend book
            // use chatmodel(openAi) declared above
            // use prompt declared above

            const combineDocsChain = await createStuffDocumentsChain({
                llm: chatModel,
                prompt
            });

            // if (vectorstore) {
            //     console.log("Local vector database", vectorstore);
            // }


            // 4 - Retrieve data from VectorDatabase
            // This will allow that search, recommendation scome from the 
            //vectorstore we just created
            const retriever = vectorstore.asRetriever();
            const retrievalChain = await createRetrievalChain({
                retriever,
                combineDocsChain
            });

            // 5 - make the system conversational
            const result = await retrievalChain.invoke({ input: userInput })
            // console.log("Local vector database",vectorstore)
            // const resultMetadata = vectorstore.similaritySearch()
            // console.log("==============================");
            // console.log("answer isbn",result.answer.isbn);
            // console.log(result)
            //  const ansObj = {
            //     answer:result.answer,
            //     isbn:result.context[0].metadata.isbn
            //  }
            res.json(result);
            // res.json(books);

            // res.json(books);
        } else {
            res.json({ message: "No data found" });
        }

        // return response as Json 
        // res.json(response);
    } catch (error) {
        console.log("Sorry, error getting books", error)
    }
    // Function to get data from the book
    // res.json(books);
}