using OpenAI;
using OpenAI.Chat;
using OpenAI.Responses;
using System.ClientModel;

namespace AI_Hello_World
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Get this API key from the user's environment variables.
            string? apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User);
            if (String.IsNullOrEmpty(apiKey) )
            {
                Console.WriteLine("API key not found. Exiting...");
                return;
            }

            //Create the chat client using the API key and the model
            OpenAIClient aiClient = new OpenAIClient(apiKey);
            ChatClient chatClient = aiClient.GetChatClient("gpt-4o-mini");

            // Set the role of AI assistant, i.e, how it should behave, what it needs, and how it should respond. 
            ChatMessage aiRole = ChatMessage.CreateSystemMessage("""
                You are a friendly weather reporter who helps people understand the weather forecast in their area
                and you provide recommendations for how to dress comfortably for the day's weather conditions.
                When helping people out, you always ask them for this information to inform the dressing recommendation
                you provide:

                1. The location (city, province, country etc.) for which they want to get the weather conditions.
                2. The type of activity they will be doing outside.

                You will then provide:
                
                1. The temperature in celsius and other weather and air quality conditions.
                2. Three suggestions for dressing for the weather that range from most comfortable to least 
                comfortable and the ease with which they will be able to carry out their activity in that clothing from most 
                difficult to least difficult.
                3. Your recommendation for the best one of the three choices to choose from to stay comfortable in the given
                weather and be able to easily carry out the activity.
            """);

            // Add the role of the AI assistant to the chat history.
            List<ChatMessage> chatHistory = new List<ChatMessage>();
            chatHistory.Add(aiRole);

            Console.WriteLine("""
                    Welcome to the AI Weather Forecaster! Let me know where you are going and what you'd like to do today.
                    Type EXIT to end the chat.
                    """);

            // Keep the conversation going until the user says EXIT.
            while (true)
            {
                //Get user input
                string? userPrompt = Console.ReadLine();
                if (!String.IsNullOrEmpty(userPrompt) && userPrompt.Equals("EXIT"))
                    break;

                // Create message based on user input
                ChatMessage userMessage = ChatMessage.CreateUserMessage(userPrompt);
                chatHistory.Add(userMessage);

                // Get the response based on current set of messages in conversation.
                ClientResult<ChatCompletion> result = chatClient.CompleteChat(chatHistory.ToArray());

                // Write the response
                Console.WriteLine(result.Value.Content[0].Text);
            }

            Console.WriteLine("Thank you for stopping by. Good bye!");
        }
    }
}
