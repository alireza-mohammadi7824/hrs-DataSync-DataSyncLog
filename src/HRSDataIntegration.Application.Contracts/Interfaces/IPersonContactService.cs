using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Interfaces
{
    public interface IPersonContactService
    {
        void ConvertSqlInsertIntoPersoncontactToOracletable(string Id);
        void ConvertSqlUpdatePersoncontactToOracletable(string Id);
        void ConvertSqlDeletePersoncontactToOracletable(string Id);
    }
}
