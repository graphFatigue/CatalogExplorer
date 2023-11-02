using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum
{
    public enum StatusCode
    {
        UserNotFound = 0,
        CatalogNotFound = 10,
        OK = 200,
        NotFound = 404,
        InternalServerError = 500,
    }
}
