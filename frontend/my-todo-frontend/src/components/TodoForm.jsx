import React, { useState, useEffect } from "react";

export default function TodoForm({ initial, onSubmit, onCancel }) {
  const [title, setTitle] = useState("");

  useEffect(() => {
    if (initial) setTitle(initial.title);
    else setTitle("");
  }, [initial]);

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!title.trim()) return;
    onSubmit({ title, isDone: initial?.isDone ?? false });
    setTitle("");
  };

  return (
    <form onSubmit={handleSubmit} className="todo-form">
      <input
        type="text"
        placeholder="Enter todo"
        value={title}
        onChange={(e) => setTitle(e.target.value)}
      />
      <button type="submit">{initial ? "Update" : "Add"}</button>
      {initial && <button type="button" onClick={onCancel}>Cancel</button>}
    </form>
  );
}
