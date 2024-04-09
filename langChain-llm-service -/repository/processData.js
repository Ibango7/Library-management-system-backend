import { OpenAIEmbeddings } from '@langchain/openai';
import { MemoryVectorStore } from 'langchain/vectorstores/memory';
import { createRetrievalChain } from "langchain/chains/retrieval";
import { createHistoryAwareRetriever } from "langchain/chains/history_aware_retriever";
import { MessagesPlaceholder } from "@langchain/core/prompts";
import { ChatPromptTemplate } from "@langchain/core/prompts";
import { ChatOpenAI } from "@langchain/openai";
import { createStuffDocumentsChain } from "langchain/chains/combine_documents";
import { VectorStore } from 'langchain/vectorstores/base';
import { LLM } from 'langchain/llms/base';

const chatModel = new ChatOpenAI({
    openAIApiKey: "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",
  });

  const prompt =
  ChatPromptTemplate.fromTemplate(` You are an expert in recommending books.
  Answer the following question based only on the provided context. Do not answer based on the general knweledge tou have
    <context>
    {context}
    </context>

    Question: {input}`);

// const key = {openAIApiKey:"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"},
const processData = async (data) => {
    // 1 - get data and  concatenate data
    let title = data[3].title;
    let description = data[3].description;
    let Author = data[3].author;
    let isbn = data[3].isbn;
    let documents = [title+"\n"+description]
    console.log("title:",title);
    console.log("Book:",description);
    console.log("Author:",Author);

    // 2 - Embed data and store in vector database
    const vectorstore = await MemoryVectorStore.fromTexts(documents,[{isbn:isbn}], new OpenAIEmbeddings({
        openAIApiKey: "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",
    }));

    // 3- Using the llm for chaining
    const combineDocsChain = await createStuffDocumentsChain({
        llm: chatModel,
        prompt
    });

    // 4- define retrieval chain 
    const retriever = vectorstore.asRetriever();
    const retrievalChain = await createRetrievalChain({
        retriever,
        combineDocsChain
    });

    // 5 - conversational 

    const response = await retrievalChain.invoke({input:" Suggest a book on human freedom"})

    console.log("response::::", response.answer);
    console.log("vectorstore::::", vectorstore);
}
export default processData 
