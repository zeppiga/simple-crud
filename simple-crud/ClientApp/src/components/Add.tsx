import React, { useState } from 'react';
import { Alert, AlertType } from "./Alert";

export function Add() {
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [alertState, setAlertState] = useState({show: false, alertType: AlertType.Info, message: ""});

  function handleSubmit(event: any) {
    setIsLoading(true);

    fetch('novelty/add', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({ name, description })
    }).then(res => {

      if (res.status !== 201) {
        setAlertState({ show: true, alertType: AlertType.Error, message: "Failed to add novelty." })
        setIsLoading(false);
        return;
      }

      setAlertState({ show: true, alertType: AlertType.Info, message: "You novelty was successfully saved." })
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

  function getAlertState(alertState: {show: boolean, alertType: AlertType, message: string}) {
      return {
        show: alertState.show, message: alertState.message, alertType: alertState.alertType, onAlertClose: () => setAlertState(prev => ({...prev, show: false}))
      }
  }

  return (
    <>
      <Alert {...getAlertState(alertState)}></Alert>
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