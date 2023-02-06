using BlazorApp1.Data;
using BlazorApp1.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using System;

namespace BlazorApp1.Pages
{
    public partial class ToDoList
    {
        List<ToDoModel> toDoList = new List<ToDoModel>();
        [Inject] public ToDoService toDoService { get; set; } = default;

        ToDoModel toDoItem = new ToDoModel();
        ToDoFilter toDoFilter = new ToDoFilter();

        private bool IsSortedAscending;
        private string CurrentSortColumn;

        protected override async Task OnInitializedAsync()
        {
            toDoList = await toDoService.GetToDoListAsync();
            toDoFilter = new ToDoFilter
            {
                StartDate = DateTime.Now,
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))
            };
        }

        private string GetSortStyle(string columnName)
        {
            if (CurrentSortColumn != columnName)
            {
                return string.Empty;
            }
            if (IsSortedAscending)
            {
                return "caret-top";
            }
            else
            {
                return "caret-bottom";
            }
        }
        private void SortColumn(string columnName)
        {
            if (columnName != CurrentSortColumn)
            {
                toDoList = toDoList.OrderBy(x => x.GetType().GetProperty(columnName).GetValue(x, null)).ToList();
                CurrentSortColumn = columnName;
                IsSortedAscending = true;
            }
            else
            {
                if (IsSortedAscending)
                {
                    toDoList = toDoList.OrderByDescending(x => x.GetType().GetProperty(columnName).GetValue(x, null)).ToList();
                }
                else
                {
                    toDoList = toDoList.OrderBy(x => x.GetType().GetProperty(columnName).GetValue(x, null)).ToList();
                }
                IsSortedAscending = !IsSortedAscending;
            }
        }
        private async Task SearchToDo()
        {
            toDoList = await toDoService.GetToDoListFilterAsync(toDoFilter);

            StateHasChanged();
        }
        private async Task ToggleToDo(int id)
        {
            var toDo = toDoList.First(x => x.Id == id);
            toDo.Status = !toDo.Status;

            await toDoService.UpdateToDoItem(toDo);
            StateHasChanged();
        }

        private async Task RemoveTodo(int id)
        {
            var toDo = toDoList.First(x => x.Id == id);
            toDoList.Remove(toDo);
            await toDoService.DeleteToDoItem(toDo);
            StateHasChanged();
        }

        private async Task AddToDo()
        {

            await toDoService.InsertToDoItem(toDoItem);
            toDoItem = new ToDoModel();
            StateHasChanged();
        }
    }
}
