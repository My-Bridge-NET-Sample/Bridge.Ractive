﻿using System.Linq;
using Bridge.Redux;
using System;

namespace Bridge.Ractive.Example
{
    public static class Todos
    {
        private static TodoAppState AddTodo(TodoAppState state, AddTodo act)
        {
            int nextId = 0;
            if (state.Todos.Any())
            {
                var maxId = state.Todos.Max(_todo => _todo.Id);
                nextId = maxId + 1;
            }

            var todo = new Todo
            {
                Id = nextId,
                Description = state.DescriptionInput,
                IsCompleted = false
            };

            return new TodoAppState
            {
                Todos = state.Todos.Concat<Todo>(new Todo[] { todo }).ToArray(),
                Visibility = state.Visibility,
                DescriptionInput = state.DescriptionInput
            };
        }


        static TodoAppState DeleteTodo(TodoAppState state, DeleteTodo act) => new TodoAppState
        {
            Todos = state.Todos.Where(todo => todo.Id != act.Id).ToArray(),
            Visibility = state.Visibility,
            DescriptionInput = state.DescriptionInput
        };


        static TodoAppState UpdateTodo(TodoAppState state, UpdateTodo act) => new TodoAppState
        {
            Todos = state.Todos.Select(todo =>
            {
                if (todo.Id == act.Id)
                {
                    return new Todo
                    {
                        Id = todo.Id,
                        Description = act.Description,
                        IsCompleted = todo.IsCompleted
                    };
                }
                else
                {
                    return todo;
                }
            }).ToArray(),

            Visibility = state.Visibility,

            DescriptionInput = state.DescriptionInput
        };


        static TodoAppState ToggleTodoCompleted(TodoAppState state, ToggleTodoCompleted act) => new TodoAppState
        {
            Todos = state.Todos.Select(todo =>
            {
                if (todo.Id == act.Id)
                {
                    return new Todo
                    {
                        Id = todo.Id,
                        Description = todo.Description, 
                        IsCompleted  = !todo.IsCompleted
                    };
                }
                else
                {
                    return todo;
                }
            }).ToArray(),
            
            Visibility = state.Visibility,
            DescriptionInput = state.DescriptionInput 
        };


        static TodoAppState UpdateDescriptionInput(TodoAppState state, UpdateDescriptionInput act)
        {
            return new TodoAppState
            {
                Todos = state.Todos,
                Visibility = state.Visibility,
                DescriptionInput = act.Description
            };
        }

        static TodoAppState SetVisibility(TodoAppState state, SetVisibility act)
        {
            return new TodoAppState
            {
                Todos = state.Todos,
                Visibility = act.Visibility,
                DescriptionInput = state.DescriptionInput
            };
        }

        public static ReduxReducer<TodoAppState> Reducer(TodoAppState initialState) => 
                BuildReducer.For<TodoAppState>()
                            .WhenActionHasType<AddTodo>(AddTodo)
                            .WhenActionHasType<DeleteTodo>(DeleteTodo)
                            .WhenActionHasType<UpdateTodo>(UpdateTodo)
                            .WhenActionHasType<ToggleTodoCompleted>(ToggleTodoCompleted)
                            .WhenActionHasType<SetVisibility>(SetVisibility)
                            .WhenActionHasType<UpdateDescriptionInput>(UpdateDescriptionInput)
                            .WhenStateIsUndefinedOrNull(() => initialState)  
                            .WhenActionIsUnknown(state => state)
                            .Build();
    }
}