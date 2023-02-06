using BlazorApp1.Models;
using BlazorApp1.Pages;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Data
{
    public class ToDoService
    {
        private readonly AppDBContext _appDBContext;
        public ToDoService(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        public async Task<List<ToDoModel>> GetToDoListFilterAsync(ToDoFilter filter)
        {
            var toDoList = new List<ToDoModel>();

            toDoList = await _appDBContext.ToDoList.Where(t => t.ToDoDate >= filter.StartDate.Date && t.ToDoDate <= filter.EndDate.Date).ToListAsync();

            if (!string.IsNullOrEmpty(filter.IsCompleted))
            {
                var status = Convert.ToBoolean(filter.IsCompleted);
                toDoList = toDoList.Where(t => t.Status == status).ToList();
            }

            if (!string.IsNullOrEmpty(filter.Title))
            {
                toDoList = toDoList.Where(t => t.Title.ToLower().Contains(filter.Title.ToLower())).ToList();
            }
            return toDoList;
        }

        public async Task<List<ToDoModel>> GetToDoListAsync()
        {
            return await _appDBContext.ToDoList.ToListAsync();
        }

        public async Task<ToDoModel> GetToDoById(int id)
        {
            return await _appDBContext.ToDoList.FirstOrDefaultAsync(t => t.Id == id) ?? new ToDoModel();
        }

        public async Task<int> InsertToDoItem(ToDoModel model)
        {
            await _appDBContext.ToDoList.AddAsync(model);
            return await _appDBContext.SaveChangesAsync();
        }

        public async Task<int> UpdateToDoItem(ToDoModel model)
        {
            var toDo = await GetToDoById(model.Id);
            if (toDo is null)
            {
                throw new Exception($"To Do item Id {model.Id} not found.");
            }
            toDo.Title = model.Title;
            toDo.ToDoDate = model.ToDoDate;
            toDo.Status = model.Status;
            return await _appDBContext.SaveChangesAsync();
        }

        public async Task<int> DeleteToDoItem(ToDoModel model)
        {
            var toDo = await GetToDoById(model.Id);
            if (toDo is null)
            {
                throw new Exception($"To Do item Id {model.Id} not found.");
            }
            _appDBContext.ToDoList.Remove(toDo);
            return await _appDBContext.SaveChangesAsync();
        }
    }
}
