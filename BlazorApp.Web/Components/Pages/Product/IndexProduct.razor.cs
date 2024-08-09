using BlazorApp.Model.Entities;
using BlazorApp.Model.Models;
using BlazorApp.Web.Components.BaseComponents;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace BlazorApp.Web.Components.Pages.Product
{
    public partial class IndexProduct
    {
        [Inject]
        public ApiClient ApiClient { get; set; }
        public List<ProductModel> ProductModels { get; set; }
        public AppModal Modal { get; set; }
        public int DeleteID { get; set; }
        [Inject]
        private IToastService ToastService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadProduct();
        }
        protected async Task LoadProduct()
        {
            var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/Product");
            if (res != null && res.Success)
            {
                ProductModels = JsonConvert.DeserializeObject<List<ProductModel>>(res.Data.ToString());
            }
        }
        protected async Task HandleDelete()
        {
            var res = await ApiClient.DeleteAsync<BaseResponseModel>($"/api/Product/{DeleteID}");
            if (res != null && res.Success)
            {
                ToastService.ShowSuccess("Delete product successfully");
                await LoadProduct();
                Modal.Close();
            }
        }
    }
}
