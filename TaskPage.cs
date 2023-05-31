using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePW
{
    public class TaskPage
    {
        static public async Task<IPage?> Login(string Login,string Password, HhPages hh, IBrowserContext context)
        {
            IPage? page = await context.NewPageAsync();
            try
            {
                await page.GotoAsync(hh.Login,new PageGotoOptions() { Timeout = 40000 });
                await page.ClickAsync(hh.ButtonQa("expand-login-by-password"));
                await page.TypeAsync(hh.InputQa("login-input-username"), Login);
                await page.TypeAsync(hh.InputQa("login-input-password"), Password);
                await page.ClickAsync(hh.ButtonQa("account-login-submit"));

                var incorrectPassword = await page.IsVisibleAsync(hh.DivQa("account-login-error"));
                if (incorrectPassword)
                {
                    throw new Exception("Incorrect password");
                }
            }
            catch (Exception ex)
            {
                Logs.Write("Не удалось залогиниться: "+ex.Message);               
                return null;
            }
            return page;
        }

        static public async Task<IPage?> SearchVacancies(IPage? page, string search,HhPages hh,IBrowserContext context)
        {
            try
            {
                await page.GotoAsync(hh.Search(search), new PageGotoOptions() { Timeout = 40000 });
                return page;
            }
            catch (Exception ex)
            {
                Logs.Write("Не удалось открыть вакансию: " + ex.Message);               
                return null;
            }

        }

        static public async Task<string?> ResponseVacancy(ILocator? element,HhPages hh,IBrowserContext context)
        {
            try
            {
                IPage? page = await context.NewPageAsync();
                string? v = await element.GetAttributeAsync("href");
                await page.GotoAsync(v, new PageGotoOptions() { Timeout = 40000 });
                //не кликается
                await page.ClickAsync(hh.ButtonQa("vacancy-response-link-top"), 
                      new PageClickOptions(){ Timeout = 40000, Delay = 10000, Force = true });               
                return v;
            }
            catch(Exception ex )
            {
                Logs.Write("Не удалось ответить: " + ex.Message);
                return null;
            }
        }
        

        static public async Task<IPage?> NextPage(IPage? page, HhPages hh, IBrowserContext context)
        {
            try
            {  //не кликается
                await page.ClickAsync(hh.ButtonQa("pager-next"), new PageClickOptions() { Timeout = 40000, Delay =10000, Force = true });
                return page;
            }
            catch (Exception ex)
            {
                Logs.Write("Не удалось перейти на следующую страницу: " + ex.Message);
                return null;
            }
        }
    }
}
