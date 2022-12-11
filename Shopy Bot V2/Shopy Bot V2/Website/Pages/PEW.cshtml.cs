using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Pages
{
    public class PEW : PageModel
    {
        private readonly ILogger<PEW> _logger;

        public PEW(ILogger<PEW> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
