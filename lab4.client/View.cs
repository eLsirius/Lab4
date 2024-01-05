using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

namespace lab4.client
{
        public class View
        {

            private bool programmRunning;

            public void Greet()
            {
                Console.WriteLine("Usage: write the following command to do something: ");
            }

            public void RepeatGreet()
            {
                WriteDashLine();
                Console.WriteLine("add to add new todo task");
                Console.WriteLine("search to search all tasks with urgency");
                Console.WriteLine("delete to delete todo task");
                Console.WriteLine("show to show all names of todo tasks");
                Console.WriteLine("last to show last added todo task");
                Console.WriteLine("quit to exit");
                WriteDashLine();

                string userInput = Console.ReadLine();
                ProcessInput(userInput);
            }
            public void WriteDashLine()
            {
                Console.WriteLine("-------");
            }
            public void Start()
            {
                Greet();
                programmRunning = true;
                while (programmRunning)
                    RepeatGreet();
            }

            public void ProcessInput(string input)
            {
                var validInputs = new Dictionary<string, int>()
        {
            { "add", 0},
            { "search", 1},
            { "quit", 3},
            { "delete", 4},
            { "xdd", 5},
            { "last", 7},
            { "show", 6 }
        };
                if (!validInputs.ContainsKey(input))
                {
                    WriteWrongInputMessage();
                    return;
                }
                switch (validInputs[input])
                {
                    case 0:
                        
                    AddTodo();
                    break;
                    case 1:
                    ShowTodoUrgency();
                        break;
                    case 2:
                        break;
                    case 3:
                        Quit();
                        break;
                    case 4:
                    DeleteTodo();
                        break;
                    case 5:
                        xdding();
                        break;
                    case 6:
                    ShowTodo();
                      break;
                    case 7:
                    ShowLast();
                    break;
                    default:
                        WriteNoInputMessage();
                        break;
                }
            }

            void WriteNoInputMessage()
            {
                Console.WriteLine("No input.");
            }

            void WriteWrongInputMessage()
            {
                Console.WriteLine("Wrong input.");
                WriteDashLine();
            }
            private void xdding()
            {
                for (int i = 0; i < 60000; i++)
                {
                    Console.Write("xdd");
                }
            }

            private void Quit()
            {
                programmRunning = false;
            }



        async private void ShowTodo()
        {
            Console.WriteLine("Loading...");
            HttpClient client = new();
            client.BaseAddress = new Uri("http://localhost:5030");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("api/todoTask");
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                var todoTasks = await response.Content.ReadFromJsonAsync<IEnumerable<TodoTaskDto>>();
                if (todoTasks.Any())
                {
                    foreach (var todoTask in todoTasks)
                    {
                        Console.WriteLine(todoTask.Name);
                    }
                }
                else
                {
                    Console.WriteLine("No data to display");
                }

            }
            else
            {
                Console.WriteLine("Can't fetch data...");
            }
        }
        async private void DeleteTodo()
        {
            Console.WriteLine("Id of deleting Todo:");
            string userInput  = Console.ReadLine();
            HttpClient client = new();
            client.BaseAddress = new Uri("http://localhost:5030");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.DeleteAsync("api/todoTask/" + userInput);
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully deleted");
            }
            else
            {
                Console.WriteLine("Can't delete data...");
            }
        }

        async private void ShowTodoUrgency()
        {
            Console.WriteLine("Urgency of todo: (Enter number: 0 - Low, 1 - Medium, 2 - High)");
            string userInput = Console.ReadLine();

            HttpClient client = new();
            client.BaseAddress = new Uri("http://localhost:5030");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("api/TodoTask/searchurgency/" + userInput);
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                var todoTasks = await response.Content.ReadFromJsonAsync<IEnumerable<TodoTaskDto>>();
                if (todoTasks.Any())
                {
                    foreach (var todoTask in todoTasks)
                    {
                        Console.WriteLine(todoTask.Name);
                    }
                }
                else
                {
                    Console.WriteLine("No data to display");
                }

            }
            else
            {
                Console.WriteLine("Can't fetch data...");
            }
        }

        static async Task<Uri> AddTodo()
        {
            HttpClient client = new();
            client.BaseAddress = new Uri("http://localhost:5030");
            client.DefaultRequestHeaders.Accept.Clear();

            TodoTaskDto todoTask = new TodoTaskDto();

            Console.WriteLine("Input id (zero for default):");
            todoTask.Id  = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Input name:");
            todoTask.Name = Console.ReadLine();

            Console.WriteLine("Input description:");
            todoTask.Description = Console.ReadLine();

            Console.WriteLine("Input priority: (0 - Low, 1 - Medium, 2 - High)");
            todoTask.Priority = (Priority)Int32.Parse(Console.ReadLine());

            todoTask.Created = DateTime.Now;


            HttpResponseMessage response = await client.PostAsJsonAsync("api/todoTask", todoTask);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
                Console.WriteLine("Added successfully.");
            else
            { 
                Console.WriteLine("Failed to add...");
            }


                return response.Headers.Location;
        }

        static async void ShowLast()
        {
            HttpClient client = new();
            client.BaseAddress = new Uri("http://localhost:5030");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("api/TodoTask/searchlast");
            if (response.IsSuccessStatusCode)
            {
                TodoTaskDto result = await response.Content.ReadFromJsonAsync<TodoTaskDto>();
                Console.WriteLine(result.Name);
            }
        }
    }
}
