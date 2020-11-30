import React, { useState } from 'react';
import { Alert, AlertProps } from "./Alert";

export function Add() {
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [showAlert, setShowAlert] = useState(false);

  function handleSubmit(event: any) {
    setIsLoading(true);

    fetch('novelty/add', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({name, description})
    }).then(() => {
      setShowAlert(true);
      setIsLoading(false);
    })

    event.preventDefault();
  }
  
  function handleNameChange(event: any) {
    const value: string = event.target.value;
    setName(value);
  }

  function handleDescriptionChange(event: any) {
    const value: string = event.target.value;
    setDescription(value);
  }

  if (isLoading) {
    return <div>loading...</div>
  }

  function getAlertState(show: boolean) {
      return {
        show: showAlert, message:"Your novelty was sucessfully added!", hideAfterSeconds: 10, onAlertClose: () => setShowAlert(false)
      }
  }

  return (
    <>
      {/* <Alert {...{show: showAlert, message:"Your novelty was sucessfully added!", hideAfterSeconds: 2, onAlertClose: () => setShowAlert(false)}}></Alert> */}
      <Alert {...getAlertState(showAlert)}></Alert>
      <form onSubmit={handleSubmit}>
    <div className="form-group">
      <label>Name of your awesome novelty</label>
      <input type="text" className="form-control" onChange={handleNameChange}></input>
      <small className="form-text text-muted">You'll identify your novelty further by it's name, so pick carefully.</small>
    </div>
    <div className="form-group">
      <label>Description</label>
      <textarea className="form-control" rows={20} onChange={handleDescriptionChange}></textarea>
    </div>
    <button type="submit" className="btn btn-primary">Add your awesome novelty!</button>
    </form>
    </>
  );
  }