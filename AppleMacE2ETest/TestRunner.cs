using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace AppleE2ETest
{
    public class TestRunner
    {
        public static async Task RunTestAsync()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new()
            {
                Headless = true
            });

            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            Console.WriteLine("🔗 Переходимо на сторінку Apple Mac...");
            await page.GotoAsync("https://www.apple.com/mac/", new() { WaitUntil = WaitUntilState.NetworkIdle });

            Console.WriteLine("\n🧪 Починаємо перевірку продуктів:");
            var failed = false;

            foreach (var product in ProductList.Products)
            {
                var locator = page.Locator($"text={product}");
                var count = await locator.CountAsync();

                if (count > 0)
                {
                    Console.WriteLine($"✅ Знайдено: {product}");
                }
                else
                {
                    Console.WriteLine($"❌ НЕ знайдено: {product}");
                    failed = true;
                }
            }

            if (failed)
            {
                // Зберегти скріншот на випадок помилки
                await page.ScreenshotAsync(new() { Path = "failure.png", FullPage = true });
                throw new Exception("❌ Деякі продукти не знайдено або сталася помилка!");
            }

            Console.WriteLine("\n🎉 Усі продукти знайдено успішно!");
        }
    }
}
