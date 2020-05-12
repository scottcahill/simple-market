using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SmWeb.Models;
using SmWeb.Services;

namespace SmWeb.Pages
{
    public class AddStoreBase : ComponentBase
    {
        [Inject]
        public IStoreDS StoreDS { get; set; }

        [Parameter]
        public string StoreId { get; set; }

        public Store Store { get; set; } = new Store();

        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;

        protected override async Task OnInitializedAsync()
        {
            Saved = false;

            if (string.IsNullOrEmpty(StoreId))//new Store is being created
            {
                //add some defaults
                Store = new Store { DateCreated = DateTime.UtcNow };
            }
            else
            {
                Store = await StoreDS.GetStoreById(StoreId);
            }
        }

        protected async Task HandleValidSubmit()
        {
            Saved = false;

            if (Store.Id == null) //new
            {
                var success = await StoreDS.AddStore(Store);
                if (success)
                {
                    StatusClass = "alert-success";
                    Message = "New Store added successfully.";
                    Saved = true;
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = "Something went wrong adding the new Store. Please try again.";
                    Saved = false;
                }
            }
            else
            {
                //await EmployeeDataService.UpdateEmployee(Employee);
                //StatusClass = "alert-success";
                //Message = "Employee updated successfully.";
                //Saved = true;
            }
        }
    }
}
