using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SmWeb.Models;
using SmWeb.Services;

namespace SmWeb.Pages
{
    public class AddMemberBase: ComponentBase
    {

        [Inject]
        public IMemberDS MemberDS { get; set; }

        [Parameter]
        public string MemberId { get; set; }

        public Member Member { get; set; } = new Member();

        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;


        protected override async Task OnInitializedAsync()
        {
            Saved = false;

            if (string.IsNullOrEmpty(MemberId))//new member is being created
            {
                //add some defaults
                Member = new Member {  DateCreated=DateTime.UtcNow };
            }
            else
            {
                Member = await MemberDS.GetMemberById(MemberId);
            }
        }

        protected async Task HandleValidSubmit()
        {
            Saved = false;

            if (Member.Id == null) //new
            {
                var success = await MemberDS.AddMember(Member);
                if (success)
                {
                    StatusClass = "alert-success";
                    Message = "New member added successfully.";
                    Saved = true;
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = "Something went wrong adding the new member. Please try again.";
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
                    