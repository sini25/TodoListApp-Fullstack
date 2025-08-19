import React from "react";
import TodoItem from "./TodoItem";

export default function TodoList({ todos, onEdit, onDelete, onToggle }) {
  return (
    <ul>
      {todos.map((t, index) => (
        <TodoItem
          key={t.id ?? index}
          todo={t}
          onEdit={() => onEdit && onEdit(t)}
          onDelete={() => onDelete && onDelete(t.id)}
          onToggle={() => onToggle && onToggle(t)}
        />
      ))}
    </ul>
  );
}
