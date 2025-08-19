import React, { useEffect, useState } from "react";
import API from "./api";
import TodoItem from "./components/TodoItem";

import TodoForm from "./components/TodoForm";
import TodoList from "./components/TodoList";

function parseDotNetDate(dateString) {
  if (!dateString) return null;
  const timestamp = Number(dateString.match(/\d+/)[0]); // extract number
  return new Date(timestamp);
}

export default function App() {
  const [todos, setTodos] = useState([]);
  const [editingTodo, setEditingTodo] = useState(null);
  const [error, setError] = useState("");

  //  Fetch todos
  const fetchTodos = async () => {
    try {
      const res = await API.get("/");
      const mapped = res.data.map((t) => ({
        id: t.Id,
        title: t.Title,
        isDone: t.IsDone,
        createdAt: t.CreatedAtUtc,
       // dueAt: t.DueAtUtc,
      }));

      setTodos(mapped);
    } catch (err) {
      console.error(err);
      setError("Failed to fetch todos");
    }
  };

  useEffect(() => {
    fetchTodos();
  }, []);

  // Add todo
  const addTodo = async (payload) => {
    try {
      const res = await API.post("/", payload);
      const newTodo = {
        id: res.data.id ?? res.data.Id,
        title: res.data.title ?? res.data.Title,
        isDone: res.data.isDone ?? res.data.IsDone,
        createdAt: res.data.createdAt ?? res.data.CreatedAt,
      };
      setTodos((prev) => [...prev, newTodo]);
    } catch (err) {
      console.error(err);
      setError("Failed to add todo");
    }
  };

  // Update todo
  const updateTodo = async (id, payload) => {
    try {
      await API.put(`/${id}`, payload);
      setTodos((prev) => prev.map((t) => (t.id === id ? { ...t, ...payload } : t)));
      setEditingTodo(null);
    } catch (err) {
      console.error(err);
      setError("Failed to update todo");
    }
  };

  // Delete todo
  const deleteTodo = async (id) => {
    try {
      await API.delete(`/${id}`);
      setTodos((prev) => prev.filter((t) => t.id !== id));
    } catch (err) {
      console.error(err);
      setError("Failed to delete todo");
    }
  };


  const handleEdit = (todo, field) => {
    if (field === "date") {
      // prompt for new date
      const newDate = prompt(
        "Enter new date (YYYY-MM-DDTHH:MM):",
        todo.createdAt ? new Date(todo.createdAt).toISOString().slice(0, 16) : ""
      );
      if (newDate) updateTodo(todo.id, { ...todo, createdAt: newDate });
    } else {
      // title edit
      setEditingTodo(todo);
    }
  };


  return (
    <div className="container" style={{ paddingTop: "50px" }}>
      <h1>Todo List</h1>
      <p>Please refresh the page once a new task added.</p>
      {error && <p className="error">{error}</p>}
      <TodoForm
        key={editingTodo ? `edit-${editingTodo.id}` : "add-form"}
        initial={editingTodo}
        onSubmit={async (payload) => {
          if (editingTodo) {
            await updateTodo(editingTodo.id, payload);
          } else {
            await addTodo(payload);
          }
        }}
        onCancel={() => setEditingTodo(null)}
      />
      <TodoList
        todos={todos}
        onEdit={handleEdit}
        onDelete={deleteTodo}
        onToggle={async (todo) => {
          await updateTodo(todo.id, { ...todo, isDone: !todo.isDone });
        }}
      />
    </div>


  );
}
