using ControleMedicamentos.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados
{
    public class RepositorioBase<T> where T : EntidadeBase<T>
    {
        List<T> registros;
    }
}
