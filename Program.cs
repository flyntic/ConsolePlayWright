using Microsoft.Playwright;
using System.Diagnostics;
using System.Threading;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using ConsolePW;
//У меня предложенная команда для pwsh не сработала 
//pwsh bin\Debug\net7.0\playwright.ps1 install  
// Сработал вариант со скачиванием необходимых браузеров из кода
//var exitCode = Microsoft.Playwright.Program.Main(new[] { "install" });
//if (exitCode == 0)
//{
//    throw new Exception("");
//}

int NumberLoad = 2;
List<string> vacansyResponse = new List<string>();
var hh = new HhPages("https://hh.ru");
var credentials = new
{
    Login = "",
    Password = ""
};

using var playwright = await Playwright.CreateAsync();

await using IBrowser browser = await playwright.Chromium.LaunchAsync(
     new BrowserTypeLaunchOptions { Headless=false,SlowMo=1000,Timeout=40000,
                                    Env=new KeyValuePair<string, string>[] 
                                    { new KeyValuePair<string, string>("DEBUG","pw:browser,pw:api")} 
                                  });

BrowserNewContextOptions device = playwright.Devices["Desktop Chrome"];
await using IBrowserContext context = await browser.NewContextAsync(device);


List<IPage?> pages = new List<IPage?>();
ActionThread.Run(async () => { pages.Add(await TaskPage.Login(credentials.Login,credentials.Password,hh,context)); },
                 NumberLoad, 
                 (int n) => { return pages.ToList().Exists(p => p != null); },
                 (int n) => pages.Count <= n);
IPage ? currentPage = pages.ToList().Find(p => p != null) ;
context.Pages.ToList().RemoveAll(p => p != currentPage);
if (currentPage == null)
{
    await context.CloseAsync();
    await browser.CloseAsync();
    Logs.Close();
    return;
}

pages.Clear();
ActionThread.Run(async () => { pages.Add(await TaskPage.SearchVacancies(currentPage, "программист C#",hh,context)); },
                 NumberLoad,
                (int n) => { return pages.ToList().Exists(p => p != null); }, 
                (int n) => pages.Count <= n);
currentPage = pages.ToList().Find(p => p != null);
if (currentPage == null)
{
    await context.CloseAsync();
    await browser.CloseAsync();
    Logs.Close();
    return;
}

  int i = 0;
  int j = 0;
do
{
    ILocator locator = currentPage.Locator(hh.AQa("serp-item__title"));
    int index = 0;

    foreach (var element in await locator.AllAsync())
    {
        if (j++ > 2) break; //отладочное ограничение
        ActionThread.Run(async () =>
       { vacansyResponse.Add(await TaskPage.ResponseVacancy(element, hh, context)); }); 
        index++;
    };

    while (vacansyResponse.Count < index) { }; //ожидание обработки всех откликов
    foreach (var v in vacansyResponse)
    {
        if (v != null)
            Logs.Write("Отклик на вакансию " + v + Environment.NewLine);
    }

    context.Pages.ToList().RemoveAll(p=>p!=currentPage); //Страницы не закрываются ?
    currentPage=await TaskPage.NextPage(currentPage,hh, context);
    
} while (currentPage!=null);

await context.CloseAsync();
await browser.CloseAsync();
Logs.Close();
return;

