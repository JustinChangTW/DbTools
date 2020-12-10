using Scriban;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbTools.Service
{
    public class TemplateService : ITemplateService
    {
        public string Render(string template, object model)
        {
            var result = Template.Parse(template).Render(model);
            return result;
        }
    }
}
