using System;
using System.Text.RegularExpressions;

namespace Core.Interfaces.Network
{
    /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="INetworkLogic"]/INetworkLogic/*'/>
    public interface INetworkLogic
    {
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="INetworkLogic"]/ConnectionStringExpr/*'/>
        Regex ConnectionStringExpr { get; }
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="INetworkLogic"]/ConnectionStringFormat/*'/>
        string ConnectionStringFormat { get; }

        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="INetworkLogic"]/Connect/*'/>
        void Connect(Func<string> getCode);
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="INetworkLogic"]/IsConnected/*'/>
        bool IsConnected();
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="INetworkLogic"]/Load/*'/>
        void Load();
        /// <include file='CoreDoc.xml' path='CoreDoc/members[@name="INetworkLogic"]/Upload/*'/>
        void Upload();
    }
}
