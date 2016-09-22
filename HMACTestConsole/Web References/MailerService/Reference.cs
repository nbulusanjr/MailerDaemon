﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace HMACTestConsole.MailerService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="MailerServiceSoap", Namespace="http://tempuri.org/")]
    public partial class MailerService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback SendOperationCompleted;
        
        private System.Threading.SendOrPostCallback SendLaterOperationCompleted;
        
        private System.Threading.SendOrPostCallback UpdateMailJobOperationCompleted;
        
        private System.Threading.SendOrPostCallback DeleteMailJobOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public MailerService() {
            this.Url = global::HMACTestConsole.Properties.Settings.Default.HMACTestConsole_MailerService_MailerService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event SendCompletedEventHandler SendCompleted;
        
        /// <remarks/>
        public event SendLaterCompletedEventHandler SendLaterCompleted;
        
        /// <remarks/>
        public event UpdateMailJobCompletedEventHandler UpdateMailJobCompleted;
        
        /// <remarks/>
        public event DeleteMailJobCompletedEventHandler DeleteMailJobCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/Send", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public MailResponse Send(Mails mails) {
            object[] results = this.Invoke("Send", new object[] {
                        mails});
            return ((MailResponse)(results[0]));
        }
        
        /// <remarks/>
        public void SendAsync(Mails mails) {
            this.SendAsync(mails, null);
        }
        
        /// <remarks/>
        public void SendAsync(Mails mails, object userState) {
            if ((this.SendOperationCompleted == null)) {
                this.SendOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendOperationCompleted);
            }
            this.InvokeAsync("Send", new object[] {
                        mails}, this.SendOperationCompleted, userState);
        }
        
        private void OnSendOperationCompleted(object arg) {
            if ((this.SendCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendCompleted(this, new SendCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendLater", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public MailResponse SendLater(Mails mails, System.DateTime datetime) {
            object[] results = this.Invoke("SendLater", new object[] {
                        mails,
                        datetime});
            return ((MailResponse)(results[0]));
        }
        
        /// <remarks/>
        public void SendLaterAsync(Mails mails, System.DateTime datetime) {
            this.SendLaterAsync(mails, datetime, null);
        }
        
        /// <remarks/>
        public void SendLaterAsync(Mails mails, System.DateTime datetime, object userState) {
            if ((this.SendLaterOperationCompleted == null)) {
                this.SendLaterOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendLaterOperationCompleted);
            }
            this.InvokeAsync("SendLater", new object[] {
                        mails,
                        datetime}, this.SendLaterOperationCompleted, userState);
        }
        
        private void OnSendLaterOperationCompleted(object arg) {
            if ((this.SendLaterCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendLaterCompleted(this, new SendLaterCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/UpdateMailJob", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public MailResponse UpdateMailJob(int mailID, string jobID, System.DateTime datetime, bool oneTimeOnly) {
            object[] results = this.Invoke("UpdateMailJob", new object[] {
                        mailID,
                        jobID,
                        datetime,
                        oneTimeOnly});
            return ((MailResponse)(results[0]));
        }
        
        /// <remarks/>
        public void UpdateMailJobAsync(int mailID, string jobID, System.DateTime datetime, bool oneTimeOnly) {
            this.UpdateMailJobAsync(mailID, jobID, datetime, oneTimeOnly, null);
        }
        
        /// <remarks/>
        public void UpdateMailJobAsync(int mailID, string jobID, System.DateTime datetime, bool oneTimeOnly, object userState) {
            if ((this.UpdateMailJobOperationCompleted == null)) {
                this.UpdateMailJobOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUpdateMailJobOperationCompleted);
            }
            this.InvokeAsync("UpdateMailJob", new object[] {
                        mailID,
                        jobID,
                        datetime,
                        oneTimeOnly}, this.UpdateMailJobOperationCompleted, userState);
        }
        
        private void OnUpdateMailJobOperationCompleted(object arg) {
            if ((this.UpdateMailJobCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UpdateMailJobCompleted(this, new UpdateMailJobCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/DeleteMailJob", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public MailResponse DeleteMailJob(int mailID, string jobID) {
            object[] results = this.Invoke("DeleteMailJob", new object[] {
                        mailID,
                        jobID});
            return ((MailResponse)(results[0]));
        }
        
        /// <remarks/>
        public void DeleteMailJobAsync(int mailID, string jobID) {
            this.DeleteMailJobAsync(mailID, jobID, null);
        }
        
        /// <remarks/>
        public void DeleteMailJobAsync(int mailID, string jobID, object userState) {
            if ((this.DeleteMailJobOperationCompleted == null)) {
                this.DeleteMailJobOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteMailJobOperationCompleted);
            }
            this.InvokeAsync("DeleteMailJob", new object[] {
                        mailID,
                        jobID}, this.DeleteMailJobOperationCompleted, userState);
        }
        
        private void OnDeleteMailJobOperationCompleted(object arg) {
            if ((this.DeleteMailJobCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeleteMailJobCompleted(this, new DeleteMailJobCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class Mails {
        
        private int idField;
        
        private string applicationGUIDField;
        
        private string fromField;
        
        private string subjectField;
        
        private string contentField;
        
        private int retriesField;
        
        private int isSentField;
        
        private string uIDField;
        
        private string[] toField;
        
        private string[] ccField;
        
        private string[] bccField;
        
        private MailAttachment[] attachmentsField;
        
        /// <remarks/>
        public int id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        public string ApplicationGUID {
            get {
                return this.applicationGUIDField;
            }
            set {
                this.applicationGUIDField = value;
            }
        }
        
        /// <remarks/>
        public string From {
            get {
                return this.fromField;
            }
            set {
                this.fromField = value;
            }
        }
        
        /// <remarks/>
        public string Subject {
            get {
                return this.subjectField;
            }
            set {
                this.subjectField = value;
            }
        }
        
        /// <remarks/>
        public string Content {
            get {
                return this.contentField;
            }
            set {
                this.contentField = value;
            }
        }
        
        /// <remarks/>
        public int Retries {
            get {
                return this.retriesField;
            }
            set {
                this.retriesField = value;
            }
        }
        
        /// <remarks/>
        public int IsSent {
            get {
                return this.isSentField;
            }
            set {
                this.isSentField = value;
            }
        }
        
        /// <remarks/>
        public string UID {
            get {
                return this.uIDField;
            }
            set {
                this.uIDField = value;
            }
        }
        
        /// <remarks/>
        public string[] To {
            get {
                return this.toField;
            }
            set {
                this.toField = value;
            }
        }
        
        /// <remarks/>
        public string[] Cc {
            get {
                return this.ccField;
            }
            set {
                this.ccField = value;
            }
        }
        
        /// <remarks/>
        public string[] Bcc {
            get {
                return this.bccField;
            }
            set {
                this.bccField = value;
            }
        }
        
        /// <remarks/>
        public MailAttachment[] Attachments {
            get {
                return this.attachmentsField;
            }
            set {
                this.attachmentsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class MailAttachment {
        
        private string filenameField;
        
        private string typeField;
        
        private float sizeField;
        
        private byte[] dataField;
        
        /// <remarks/>
        public string Filename {
            get {
                return this.filenameField;
            }
            set {
                this.filenameField = value;
            }
        }
        
        /// <remarks/>
        public string Type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
            }
        }
        
        /// <remarks/>
        public float Size {
            get {
                return this.sizeField;
            }
            set {
                this.sizeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] Data {
            get {
                return this.dataField;
            }
            set {
                this.dataField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class MailResponse {
        
        private string resultField;
        
        private long messageIDField;
        
        private string mailGUIDField;
        
        private string errorMessageField;
        
        private string jobIDField;
        
        /// <remarks/>
        public string Result {
            get {
                return this.resultField;
            }
            set {
                this.resultField = value;
            }
        }
        
        /// <remarks/>
        public long MessageID {
            get {
                return this.messageIDField;
            }
            set {
                this.messageIDField = value;
            }
        }
        
        /// <remarks/>
        public string MailGUID {
            get {
                return this.mailGUIDField;
            }
            set {
                this.mailGUIDField = value;
            }
        }
        
        /// <remarks/>
        public string ErrorMessage {
            get {
                return this.errorMessageField;
            }
            set {
                this.errorMessageField = value;
            }
        }
        
        /// <remarks/>
        public string JobID {
            get {
                return this.jobIDField;
            }
            set {
                this.jobIDField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void SendCompletedEventHandler(object sender, SendCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public MailResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((MailResponse)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void SendLaterCompletedEventHandler(object sender, SendLaterCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendLaterCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendLaterCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public MailResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((MailResponse)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void UpdateMailJobCompletedEventHandler(object sender, UpdateMailJobCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UpdateMailJobCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UpdateMailJobCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public MailResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((MailResponse)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void DeleteMailJobCompletedEventHandler(object sender, DeleteMailJobCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DeleteMailJobCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal DeleteMailJobCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public MailResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((MailResponse)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591