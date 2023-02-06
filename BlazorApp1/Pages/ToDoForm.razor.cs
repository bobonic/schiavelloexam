using BlazorApp1.Data;
using BlazorApp1.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorApp1.Pages
{
    public partial class ToDoForm
    {
        [Parameter] public int id { get; set; }
        protected string Title = "Add";
        protected ToDoModel toDoItem = new();

        [Inject] public ToDoService toDoService { get; set; } = default;
        protected override async Task OnParametersSetAsync()
        {
            if (id != 0)
            {
                Title = "Edit";
                toDoItem = await toDoService.GetToDoById(id);
            }
            else
            {
                toDoItem.ToDoDate = DateTime.Now;
            }
        }
        protected async Task SaveToDoItem()
        {
            if (toDoItem.Id != 0)
            {
                await toDoService.UpdateToDoItem(toDoItem);
            }
            else
            {
                await toDoService.InsertToDoItem(toDoItem);
            }
            Cancel();
        }
        public void Cancel()
        {
            NavigationManager.NavigateTo("/todolist");
        }
    }
}
