using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePW
{
    public class HhPages
    {
        public string Host { get; }

        public HhPages(string host)
        {
            Host = host ?? throw new ArgumentNullException(nameof(host));
        }

        public string Login => $"{Host}/account/login";
        public string Resume => $"{Host}/resume";
        public string SearchVacancies => $"{Host}/search/vacancy";

        public string Vacancy(int id) => $"{Host}/vacancy/{id}";
        public string Search(string query)
        {
            var escapedQuery = Uri.EscapeDataString(query);
            return $"{SearchVacancies}?text={escapedQuery}" +
                   "&search_field=name&search_field=company_name&search_field=description";
        }

        public string ButtonQa(string dataQa) => $"button[data-qa='{dataQa}']";
        public string InputQa(string dataQa) => $"input[data-qa='{dataQa}']";

        public string DivQa(string dataQa) => $"div[data-qa='{dataQa}']";
        public string AQa(string dataQa) => $"a[data-qa='{dataQa}']";

    }


}
