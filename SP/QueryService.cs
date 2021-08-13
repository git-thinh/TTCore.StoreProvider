[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName = "IQueryService")]
public interface IQueryService
{

    ////[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/Batch", ReplyAction = "http://tempuri.org/IQueryService/BatchResponse")]
    ////System.Collections.Generic.List<Mascot.SP.Solutions.Model.Batch.BatchOutput> Batch(Mascot.SP.Solutions.Model.Batch.BatchContract input);

    ////[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/Batch", ReplyAction = "http://tempuri.org/IQueryService/BatchResponse")]
    ////System.Threading.Tasks.Task<System.Collections.Generic.List<Mascot.SP.Solutions.Model.Batch.BatchOutput>> BatchAsync(Mascot.SP.Solutions.Model.Batch.BatchContract input);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/Query", ReplyAction = "http://tempuri.org/IQueryService/QueryResponse")]
    string Query(string listName, string viewFields, string select, string filter, string join, string projectedFields, int top);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/Query", ReplyAction = "http://tempuri.org/IQueryService/QueryResponse")]
    //System.Threading.Tasks.Task<string> QueryAsync(string listName, string viewFields, string select, string filter, string join, string projectedFields, int top);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/QueryRecursive", ReplyAction = "http://tempuri.org/IQueryService/QueryRecursiveResponse")]
    string QueryRecursive(string listName, string viewFields, string select, string filter, string join, string projectedFields, int top);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/QueryRecursive", ReplyAction = "http://tempuri.org/IQueryService/QueryRecursiveResponse")]
    //System.Threading.Tasks.Task<string> QueryRecursiveAsync(string listName, string viewFields, string select, string filter, string join, string projectedFields, int top);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/QueryWithPaging", ReplyAction = "http://tempuri.org/IQueryService/QueryWithPagingResponse")]
    //string QueryWithPaging(string listName, string viewFields, string select, string filter, string join, string projectedFields, int pageIndex, int pageSize, string sortFields);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/QueryWithPaging", ReplyAction = "http://tempuri.org/IQueryService/QueryWithPagingResponse")]
    //System.Threading.Tasks.Task<string> QueryWithPagingAsync(string listName, string viewFields, string select, string filter, string join, string projectedFields, int pageIndex, int pageSize, string sortFields);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/DeleteItems", ReplyAction = "http://tempuri.org/IQueryService/DeleteItemsResponse")]
    //string DeleteItems(string listGuid, string listName, string filter, string join, string projectedFields);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/DeleteItems", ReplyAction = "http://tempuri.org/IQueryService/DeleteItemsResponse")]
    //System.Threading.Tasks.Task<string> DeleteItemsAsync(string listGuid, string listName, string filter, string join, string projectedFields);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/DownloadFile", ReplyAction = "http://tempuri.org/IQueryService/DownloadFileResponse")]
    //string DownloadFile(string listName, int fileId);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/DownloadFile", ReplyAction = "http://tempuri.org/IQueryService/DownloadFileResponse")]
    //System.Threading.Tasks.Task<string> DownloadFileAsync(string listName, int fileId);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/UploadFile", ReplyAction = "http://tempuri.org/IQueryService/UploadFileResponse")]
    //int UploadFile(string listName, string fileContent, string fileName, bool overWrite);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/UploadFile", ReplyAction = "http://tempuri.org/IQueryService/UploadFileResponse")]
    //System.Threading.Tasks.Task<int> UploadFileAsync(string listName, string fileContent, string fileName, bool overWrite);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/Lists", ReplyAction = "http://tempuri.org/IQueryService/ListsResponse")]
    //System.Collections.Generic.List<Mascot.SP.Solutions.Model.Lists.ListInfo> Lists(bool allList);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/Lists", ReplyAction = "http://tempuri.org/IQueryService/ListsResponse")]
    //System.Threading.Tasks.Task<System.Collections.Generic.List<Mascot.SP.Solutions.Model.Lists.ListInfo>> ListsAsync(bool allList);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/UpdateCachedItem", ReplyAction = "http://tempuri.org/IQueryService/UpdateCachedItemResponse")]
    //void UpdateCachedItem(string key, int id, Mascot.SP.Solutions.Dynamic.Helper.SPRefreshCacheMode mode);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/UpdateCachedItem", ReplyAction = "http://tempuri.org/IQueryService/UpdateCachedItemResponse")]
    //System.Threading.Tasks.Task UpdateCachedItemAsync(string key, int id, Mascot.SP.Solutions.Dynamic.Helper.SPRefreshCacheMode mode);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/GetItemVersions", ReplyAction = "http://tempuri.org/IQueryService/GetItemVersionsResponse")]
    //string GetItemVersions(string listName, int itemId, string viewFields);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/GetItemVersions", ReplyAction = "http://tempuri.org/IQueryService/GetItemVersionsResponse")]
    //System.Threading.Tasks.Task<string> GetItemVersionsAsync(string listName, int itemId, string viewFields);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/GetItemVersionContent", ReplyAction = "http://tempuri.org/IQueryService/GetItemVersionContentResponse")]
    //string GetItemVersionContent(string listName, int itemId, string viewFields, int version);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/GetItemVersionContent", ReplyAction = "http://tempuri.org/IQueryService/GetItemVersionContentResponse")]
    //System.Threading.Tasks.Task<string> GetItemVersionContentAsync(string listName, int itemId, string viewFields, int version);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/SetFolder", ReplyAction = "http://tempuri.org/IQueryService/SetFolderResponse")]
    //int SetFolder(string listName, string folderName, Mascot.SP.Solutions.Model.Lists.FolderOperation operation);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/SetFolder", ReplyAction = "http://tempuri.org/IQueryService/SetFolderResponse")]
    //System.Threading.Tasks.Task<int> SetFolderAsync(string listName, string folderName, Mascot.SP.Solutions.Model.Lists.FolderOperation operation);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/MoveFolder", ReplyAction = "http://tempuri.org/IQueryService/MoveFolderResponse")]
    //bool MoveFolder(string currentFolder, string newDestination);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/MoveFolder", ReplyAction = "http://tempuri.org/IQueryService/MoveFolderResponse")]
    //System.Threading.Tasks.Task<bool> MoveFolderAsync(string currentFolder, string newDestination);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/MoveFile", ReplyAction = "http://tempuri.org/IQueryService/MoveFileResponse")]
    //bool MoveFile(string currentFilePath, string newFilePath);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/MoveFile", ReplyAction = "http://tempuri.org/IQueryService/MoveFileResponse")]
    //System.Threading.Tasks.Task<bool> MoveFileAsync(string currentFilePath, string newFilePath);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/AddUserToGroup", ReplyAction = "http://tempuri.org/IQueryService/AddUserToGroupResponse")]
    //bool AddUserToGroup(string groupName, string userName);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/AddUserToGroup", ReplyAction = "http://tempuri.org/IQueryService/AddUserToGroupResponse")]
    //System.Threading.Tasks.Task<bool> AddUserToGroupAsync(string groupName, string userName);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/RemoveUserFromGroup", ReplyAction = "http://tempuri.org/IQueryService/RemoveUserFromGroupResponse")]
    //bool RemoveUserFromGroup(string groupName, string userName);

    //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IQueryService/RemoveUserFromGroup", ReplyAction = "http://tempuri.org/IQueryService/RemoveUserFromGroupResponse")]
    //System.Threading.Tasks.Task<bool> RemoveUserFromGroupAsync(string groupName, string userName);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public interface IQueryServiceChannel : IQueryService, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public partial class QueryServiceClient : System.ServiceModel.ClientBase<IQueryService>, IQueryService
{

    public QueryServiceClient()
    {
    }

    public QueryServiceClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
    {
    }

    public QueryServiceClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
    {
    }

    public QueryServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
    {
    }

    public QueryServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
    {
    }

    //////public System.Collections.Generic.List<Mascot.SP.Solutions.Model.Batch.BatchOutput> Batch(Mascot.SP.Solutions.Model.Batch.BatchContract input)
    //////{
    //////    return base.Channel.Batch(input);
    //////}

    //////public System.Threading.Tasks.Task<System.Collections.Generic.List<Mascot.SP.Solutions.Model.Batch.BatchOutput>> BatchAsync(Mascot.SP.Solutions.Model.Batch.BatchContract input)
    //////{
    //////    return base.Channel.BatchAsync(input);
    //////}

    public string Query(string listName, string viewFields, string select, string filter, string join, string projectedFields, int top)
    {
        return base.Channel.Query(listName, viewFields, select, filter, join, projectedFields, top);
    }

    //////public System.Threading.Tasks.Task<string> QueryAsync(string listName, string viewFields, string select, string filter, string join, string projectedFields, int top)
    //////{
    //////    return base.Channel.QueryAsync(listName, viewFields, select, filter, join, projectedFields, top);
    //////}

    public string QueryRecursive(string listName, string viewFields, string select, string filter, string join, string projectedFields, int top)
    {
        return base.Channel.QueryRecursive(listName, viewFields, select, filter, join, projectedFields, top);
    }

    //public System.Threading.Tasks.Task<string> QueryRecursiveAsync(string listName, string viewFields, string select, string filter, string join, string projectedFields, int top)
    //{
    //    return base.Channel.QueryRecursiveAsync(listName, viewFields, select, filter, join, projectedFields, top);
    //}

    //////public string QueryWithPaging(string listName, string viewFields, string select, string filter, string join, string projectedFields, int pageIndex, int pageSize, string sortFields)
    //////{
    //////    return base.Channel.QueryWithPaging(listName, viewFields, select, filter, join, projectedFields, pageIndex, pageSize, sortFields);
    //////}

    //////public System.Threading.Tasks.Task<string> QueryWithPagingAsync(string listName, string viewFields, string select, string filter, string join, string projectedFields, int pageIndex, int pageSize, string sortFields)
    //////{
    //////    return base.Channel.QueryWithPagingAsync(listName, viewFields, select, filter, join, projectedFields, pageIndex, pageSize, sortFields);
    //////}

    //////public string DeleteItems(string listGuid, string listName, string filter, string join, string projectedFields)
    //////{
    //////    return base.Channel.DeleteItems(listGuid, listName, filter, join, projectedFields);
    //////}

    //////public System.Threading.Tasks.Task<string> DeleteItemsAsync(string listGuid, string listName, string filter, string join, string projectedFields)
    //////{
    //////    return base.Channel.DeleteItemsAsync(listGuid, listName, filter, join, projectedFields);
    //////}

    //////public string DownloadFile(string listName, int fileId)
    //////{
    //////    return base.Channel.DownloadFile(listName, fileId);
    //////}

    //////public System.Threading.Tasks.Task<string> DownloadFileAsync(string listName, int fileId)
    //////{
    //////    return base.Channel.DownloadFileAsync(listName, fileId);
    //////}

    //////public int UploadFile(string listName, string fileContent, string fileName, bool overWrite)
    //////{
    //////    return base.Channel.UploadFile(listName, fileContent, fileName, overWrite);
    //////}

    //////public System.Threading.Tasks.Task<int> UploadFileAsync(string listName, string fileContent, string fileName, bool overWrite)
    //////{
    //////    return base.Channel.UploadFileAsync(listName, fileContent, fileName, overWrite);
    //////}

    //////public System.Collections.Generic.List<Mascot.SP.Solutions.Model.Lists.ListInfo> Lists(bool allList)
    //////{
    //////    return base.Channel.Lists(allList);
    //////}

    //////public System.Threading.Tasks.Task<System.Collections.Generic.List<Mascot.SP.Solutions.Model.Lists.ListInfo>> ListsAsync(bool allList)
    //////{
    //////    return base.Channel.ListsAsync(allList);
    //////}

    //////public void UpdateCachedItem(string key, int id, Mascot.SP.Solutions.Dynamic.Helper.SPRefreshCacheMode mode)
    //////{
    //////    base.Channel.UpdateCachedItem(key, id, mode);
    //////}

    //////public System.Threading.Tasks.Task UpdateCachedItemAsync(string key, int id, Mascot.SP.Solutions.Dynamic.Helper.SPRefreshCacheMode mode)
    //////{
    //////    return base.Channel.UpdateCachedItemAsync(key, id, mode);
    //////}

    //////public string GetItemVersions(string listName, int itemId, string viewFields)
    //////{
    //////    return base.Channel.GetItemVersions(listName, itemId, viewFields);
    //////}

    //////public System.Threading.Tasks.Task<string> GetItemVersionsAsync(string listName, int itemId, string viewFields)
    //////{
    //////    return base.Channel.GetItemVersionsAsync(listName, itemId, viewFields);
    //////}

    //////public string GetItemVersionContent(string listName, int itemId, string viewFields, int version)
    //////{
    //////    return base.Channel.GetItemVersionContent(listName, itemId, viewFields, version);
    //////}

    //////public System.Threading.Tasks.Task<string> GetItemVersionContentAsync(string listName, int itemId, string viewFields, int version)
    //////{
    //////    return base.Channel.GetItemVersionContentAsync(listName, itemId, viewFields, version);
    //////}

    //////public int SetFolder(string listName, string folderName, Mascot.SP.Solutions.Model.Lists.FolderOperation operation)
    //////{
    //////    return base.Channel.SetFolder(listName, folderName, operation);
    //////}

    //////public System.Threading.Tasks.Task<int> SetFolderAsync(string listName, string folderName, Mascot.SP.Solutions.Model.Lists.FolderOperation operation)
    //////{
    //////    return base.Channel.SetFolderAsync(listName, folderName, operation);
    //////}

    //////public bool MoveFolder(string currentFolder, string newDestination)
    //////{
    //////    return base.Channel.MoveFolder(currentFolder, newDestination);
    //////}

    //////public System.Threading.Tasks.Task<bool> MoveFolderAsync(string currentFolder, string newDestination)
    //////{
    //////    return base.Channel.MoveFolderAsync(currentFolder, newDestination);
    //////}

    //////public bool MoveFile(string currentFilePath, string newFilePath)
    //////{
    //////    return base.Channel.MoveFile(currentFilePath, newFilePath);
    //////}

    //////public System.Threading.Tasks.Task<bool> MoveFileAsync(string currentFilePath, string newFilePath)
    //////{
    //////    return base.Channel.MoveFileAsync(currentFilePath, newFilePath);
    //////}

    //////public bool AddUserToGroup(string groupName, string userName)
    //////{
    //////    return base.Channel.AddUserToGroup(groupName, userName);
    //////}

    //////public System.Threading.Tasks.Task<bool> AddUserToGroupAsync(string groupName, string userName)
    //////{
    //////    return base.Channel.AddUserToGroupAsync(groupName, userName);
    //////}

    //////public bool RemoveUserFromGroup(string groupName, string userName)
    //////{
    //////    return base.Channel.RemoveUserFromGroup(groupName, userName);
    //////}

    //////public System.Threading.Tasks.Task<bool> RemoveUserFromGroupAsync(string groupName, string userName)
    //////{
    //////    return base.Channel.RemoveUserFromGroupAsync(groupName, userName);
    //////}
}
