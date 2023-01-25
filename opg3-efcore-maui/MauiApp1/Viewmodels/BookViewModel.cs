using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MauiApp1.Models;
using MauiApp1.Services;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MauiApp1.Viewmodels;

public partial class BookViewModel : ObservableObject
{
    private BookService _bookService;

    public BookViewModel(BookService bookService)
    {
        this._bookService = bookService;

        FetchBooks();
    }

    [ObservableProperty]
    private bool isRefreshing;

    [ObservableProperty]
    private string title;

    [ObservableProperty]
    private string author;

    [ObservableProperty]
    private string amountOfPages;

    [ObservableProperty]
    private Book selectedItem;

    [ObservableProperty]
    private ObservableCollection<Book> books = new();

    [RelayCommand]
    private async Task AddBook()
    {

        Book book = new Book
        {
            title = title,
            author = author,
            numberOfPages = int.Parse(amountOfPages),
            id = 0
        };

        // save current books
        var backupBooks = Books;
        var oldBooks = Books;

        // add new book
        oldBooks.Add(book);

        try
        {
            // Delete in database
            await this._bookService.CreateBookAsync(book);

            // overwrite old books with new list of books
            Books = oldBooks;
        }
        catch // On any exception revert changes (including timeout)
        {
            Books = backupBooks;
        }
    }

    [RelayCommand]
    private async Task DeleteBook()
    {
        if (selectedItem == null)
            return;

        // save current books
        var backupBooks = books;
        var oldBooks = books;

        // add new book
        var index = oldBooks.ToList().FindIndex(x => x == selectedItem);

        oldBooks.RemoveAt(index);

        try
        {
            // Delete in database
            await this._bookService.DeleteBookAsync(selectedItem.id);

            // overwrite old books with new list of books
            Books = oldBooks;

            selectedItem = null;
        }
        catch // On any exception revert changes (including timeout)
        {
            Books = backupBooks;
        }

    }

    [RelayCommand]
    private async Task FetchBooks()
    {
        IsRefreshing = true;

        // Save old books
        var backupBooks = Books;
        var oldBooks = Books;

        try
        {
            Books.Clear();

            // Get books from API
            var newBooks = await this._bookService.GetBooksAsync();

            // Add newBooks to oldBooks
            newBooks.ForEach(book => oldBooks.Add(book));

            // Overwrite books
            Books = oldBooks;
        }
        catch(Exception ex)
        {
            Books = backupBooks;
        }

        IsRefreshing = false;
    }
}
