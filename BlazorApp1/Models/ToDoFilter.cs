﻿namespace BlazorApp1.Models
{
    public class ToDoFilter
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string IsCompleted { get; set; }
    }
}
