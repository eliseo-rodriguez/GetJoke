#nullable enable

using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GetJoke.Pages
{
    public partial class Index : ComponentBase
    {
        //Try /random_joke, /random_ten, /jokes/random, or /jokes/ten
        private const string RequestUri = "https://official-joke-api.appspot.com/jokes/programming/random";
        JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public string TextJoke { get; set; } = "";

        public MarkupString HtmlTextJoke { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetJokeAsync();
        }

        private async Task GetJokeAsync()
        {
            using HttpClient httpClient = new();
            
            Joke[]? jokes = await httpClient.GetFromJsonAsync<Joke[]>(RequestUri, _options);
            Joke? joke = jokes?[0];

            TextJoke = joke is not null ? $"{joke.Setup}{Environment.NewLine}{joke.Punchline}"
                                        : "No joke here...";

            HtmlTextJoke = (MarkupString)TextJoke.Replace(Environment.NewLine, "<br/>");
        }

        public record Joke(int Id, string Type, string Setup, string Punchline);
    }
}
