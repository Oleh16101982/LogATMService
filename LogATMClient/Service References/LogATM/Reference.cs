﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LogATMClient.LogATM {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="LogATM.IGetDate", CallbackContract=typeof(LogATMClient.LogATM.IGetDateCallback), SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface IGetDate {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGetDate/CurrDate", ReplyAction="http://tempuri.org/IGetDate/CurrDateResponse")]
        System.DateTime CurrDate();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGetDate/Registration", ReplyAction="http://tempuri.org/IGetDate/RegistrationResponse")]
        bool Registration(string CompName, string IPAddress);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IGetDateCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGetDate/ServerQuery")]
        void ServerQuery(long QueryVal);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IGetDateChannel : LogATMClient.LogATM.IGetDate, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetDateClient : System.ServiceModel.DuplexClientBase<LogATMClient.LogATM.IGetDate>, LogATMClient.LogATM.IGetDate {
        
        public GetDateClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public GetDateClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public GetDateClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public GetDateClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public GetDateClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public System.DateTime CurrDate() {
            return base.Channel.CurrDate();
        }
        
        public bool Registration(string CompName, string IPAddress) {
            return base.Channel.Registration(CompName, IPAddress);
        }
    }
}
