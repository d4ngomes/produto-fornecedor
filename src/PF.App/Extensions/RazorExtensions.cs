using Microsoft.AspNetCore.Mvc.Razor;
using PF.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PF.App.Extensions
{
    public static class RazorExtensions
    {
        public static string FormataDocumento(this RazorPage page, TipoFornecedor tipoPessoa, string documento)
        {
            return tipoPessoa == TipoFornecedor.PessoaFisica ? Convert.ToUInt64(documento).ToString(@"000\.000\.000\-00") : Convert.ToUInt64(documento).ToString(@"00\.000\.000\/0000\-00");
        }
    }
}
