using System;
using System.Net;
using System.Text;
using System.Web;
using System.Text.Json;
using System.Net.Http.Json;
using MauiApp1.Models;

namespace MauiApp1.Services;


public class BookService
{
    HttpClient client;
    string baseUrl;

    public BookService(string url)
    {
        this.client = new HttpClient();

        // 5 Seconds timeout
        this.client.Timeout = TimeSpan.FromSeconds(5);

        this.baseUrl = url;
    }

    public async Task<List<Book>> GetBooksAsync()
    {
        var response = await this.client.GetAsync($"{baseUrl}/api/Books");

        return await response.Content.ReadFromJsonAsync<List<Book>>();
    }

    public async Task CreateBookAsync(Book book)
    {
        var response = await this.client.PostAsJsonAsync<Book>($"{baseUrl}/api/Books", book);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to create book in API");
        }
    }

    public async Task<Book> GetBookAsync(int id)
    {
        var response = await this.client.GetAsync($"{baseUrl}/api/Books/{id}");

        return await response.Content.ReadFromJsonAsync<Book>();
    }

    public async Task PutBookAsync(int id, Book book)
    {
        var response = await this.client.PutAsJsonAsync<Book>($"{baseUrl}/api/Books/{id}", book);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to update book in API");
        }
    }

    public async Task DeleteBookAsync(int id)
    {
        var response = await this.client.DeleteAsync($"{baseUrl}/api/Books/{id}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to delete book in API");
        }
    }
}
