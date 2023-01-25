﻿using MauiApp1.Services;
using MauiApp1.Viewmodels;
using Microsoft.Extensions.Logging;

namespace MauiApp1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<BookService>(new BookService(url: "http://10.0.2.2:5007"));
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<BookViewModel>();
            return builder.Build();
        }
    }
}