using System;
using System.Collections.Generic;
using System.Text;

namespace CorbadoWidgetBackend
{
    public enum UserStatus : int
    {
        Permitted = 200,
        Blocked = 403,
        DoesNotExist = 404,
        Error = 400
    }

}
