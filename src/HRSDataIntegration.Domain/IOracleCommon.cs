using System;

namespace HRSDataIntegration
{
    public interface IOracleCommon
    {
        void TBActivity_Log(string LogTableName, string ID, int Document_Code, Int64 Domain_Code);
        string ToStringDateTime(int? dateInt);
        void InsertInto_DataConverter_MappingId(string OracleId , string SqlID, string OldTableName,string OldColumnName,string newTableName, string newColumnName);
        void Update_DataConverter_MappingId(string OracleId, string SqlID, string OldTableName, string OldColumnName, string newTableName, string newColumnName);
        string Get_Old_ColumnValue(string SqlID, string OldTableName, string OldColumnName, string newTableName, string newColumnName);
        string OldColumnValue(string oldTableName , string oldColumnName , string newColumnValue);
        string DomainMappingCodeOldColumnValue(string oldTableName, string oldColumnName, string newColumnValue);
        int GetTableCount(string tableName);
    }
}