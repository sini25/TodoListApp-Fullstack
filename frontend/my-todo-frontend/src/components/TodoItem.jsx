import React from "react";

export default function TodoItem({ todo, onEdit, onDelete, onToggle }) {
  return (
    <div
      className="todo-item"
      style={{ display: "flex", gap: "10px", alignItems: "center" }}
    >
      <input
        type="checkbox"
        checked={todo.isDone}
        onChange={() => onToggle(todo)}
        style={{ width: "20px" }}
      />
      <span
        onClick={() => onEdit(todo, "title")}
        style={{ flex: 1, cursor: "pointer" }}
      >
        {todo.title}
      </span>
      <span
        onClick={() => onEdit(todo, "date")}
        style={{ width: "200px", cursor: "pointer", textDecoration: "underline" }}
      >
        {todo.createdAt ? new Date(todo.createdAt).toLocaleString() : "-"}
      </span>
      
      <button
        onClick={() => onEdit(todo)}
        style={{ padding: "5px 10px", backgroundColor: "#ffc107", color: "#000", borderRadius: "5px", border: "none", cursor: "pointer" }}
      >
        Edit
      </button>
      <button onClick={() => onDelete(todo.id)} style={{ width: "60px" }}>
        Delete
      </button>
    </div>
  );
}
