﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Asiatek.TMS.MyService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MyService.MyServiceSoap")]
    public interface MyServiceSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/LoginForm", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string LoginForm(string no, string pwd, string CompanyAccount);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetSumStrucCode", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetSumStrucCode(string StrucCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CarInfoSel", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string CarInfoSel(string Struc_Code, string VEHICLE_CODE, string Sim_Code, string Terminal_Code);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CarHistoryInfo", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string CarHistoryInfo(string VEHICLE_CODE, string fromtime, string totime, int invTime, bool isFilterVelocity);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CarExtremelyInfo", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string CarExtremelyInfo(string STRUC_CODE, string ExceptionName, int page, string fromtime, string totime, string VEHICLE_CODE, double AVelocity);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CarExteType", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string CarExteType();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CarEvenInfos", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string CarEvenInfos(string Struc_Name, string strCarNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CarMileage", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string CarMileage(string StartDate, string EndDate, string VEHICLE_CODE, string Struc_Code, int page);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/InsertJOURNALWeb", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string InsertJOURNALWeb(string VEHICLE_CODE, string STRUC_CODE, float AMOUNT, System.DateTime SHIPDATE, int JOB_NUM);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SelJournalWeb", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string SelJournalWeb(string Vehicle_code, string Startime, string Endtime, string UserStruCode, int page);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SelVersion", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string SelVersion();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SelVersionIOS", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string SelVersionIOS();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CarOil", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string CarOil(string beginTime, string endTime, string VEHICLE_CODE);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CarFristSigins", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string CarFristSigins(string VEHICLE_CODE, string fromtime, string totime, string Latitude, string Longitude);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface MyServiceSoapChannel : Asiatek.TMS.MyService.MyServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MyServiceSoapClient : System.ServiceModel.ClientBase<Asiatek.TMS.MyService.MyServiceSoap>, Asiatek.TMS.MyService.MyServiceSoap {
        
        public MyServiceSoapClient() {
        }
        
        public MyServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MyServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MyServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MyServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string LoginForm(string no, string pwd, string CompanyAccount) {
            return base.Channel.LoginForm(no, pwd, CompanyAccount);
        }
        
        public string GetSumStrucCode(string StrucCode) {
            return base.Channel.GetSumStrucCode(StrucCode);
        }
        
        public string CarInfoSel(string Struc_Code, string VEHICLE_CODE, string Sim_Code, string Terminal_Code) {
            return base.Channel.CarInfoSel(Struc_Code, VEHICLE_CODE, Sim_Code, Terminal_Code);
        }
        
        public string CarHistoryInfo(string VEHICLE_CODE, string fromtime, string totime, int invTime, bool isFilterVelocity) {
            return base.Channel.CarHistoryInfo(VEHICLE_CODE, fromtime, totime, invTime, isFilterVelocity);
        }
        
        public string CarExtremelyInfo(string STRUC_CODE, string ExceptionName, int page, string fromtime, string totime, string VEHICLE_CODE, double AVelocity) {
            return base.Channel.CarExtremelyInfo(STRUC_CODE, ExceptionName, page, fromtime, totime, VEHICLE_CODE, AVelocity);
        }
        
        public string CarExteType() {
            return base.Channel.CarExteType();
        }
        
        public string CarEvenInfos(string Struc_Name, string strCarNo) {
            return base.Channel.CarEvenInfos(Struc_Name, strCarNo);
        }
        
        public string CarMileage(string StartDate, string EndDate, string VEHICLE_CODE, string Struc_Code, int page) {
            return base.Channel.CarMileage(StartDate, EndDate, VEHICLE_CODE, Struc_Code, page);
        }
        
        public string InsertJOURNALWeb(string VEHICLE_CODE, string STRUC_CODE, float AMOUNT, System.DateTime SHIPDATE, int JOB_NUM) {
            return base.Channel.InsertJOURNALWeb(VEHICLE_CODE, STRUC_CODE, AMOUNT, SHIPDATE, JOB_NUM);
        }
        
        public string SelJournalWeb(string Vehicle_code, string Startime, string Endtime, string UserStruCode, int page) {
            return base.Channel.SelJournalWeb(Vehicle_code, Startime, Endtime, UserStruCode, page);
        }
        
        public string SelVersion() {
            return base.Channel.SelVersion();
        }
        
        public string SelVersionIOS() {
            return base.Channel.SelVersionIOS();
        }
        
        public string CarOil(string beginTime, string endTime, string VEHICLE_CODE) {
            return base.Channel.CarOil(beginTime, endTime, VEHICLE_CODE);
        }
        
        public string CarFristSigins(string VEHICLE_CODE, string fromtime, string totime, string Latitude, string Longitude) {
            return base.Channel.CarFristSigins(VEHICLE_CODE, fromtime, totime, Latitude, Longitude);
        }
    }
}