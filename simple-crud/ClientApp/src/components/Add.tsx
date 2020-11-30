import React, { useState } from 'react';
import { Alert, AlertType } from "./Alert";
import { LoadableContainer } from "./LoadableContainer";
import { post } from "../rest";

export function Add() {
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [alertState, setAlertState] = useState({show: false, alertType: AlertType.Info, message: ""});

  function handleSubmit(event: any) {
    setIsLoading(true);

    post('novelty/add', { name, description })
      .then(res => {

        if (res.statusCode !== 201) {
          setAlertState({ show: true, alertType: AlertType.Error, message: "Failed to add novelty. " + res.contents })
          setIsLoading(false);
          return;
        }

        setAlertState({ show: true, alertType: AlertType.Info, message: "You novelty was successfully saved." })
        setName("");
        setDescription("");
        setIsLoading(false);
      });

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

  function getAlertState(alertState: {show: boolean, alertType: AlertType, message: string}) {
      return {
        show: alertState.show, message: alertState.message, alertType: alertState.alertType, onAlertClose: () => setAlertState(prev => ({...prev, show: false}))
      }
  }

  return (
    <LoadableContainer {...{isLoading: isLoading}}>
      <Alert {...getAlertState(alertState)}></Alert>
      <form onSubmit={handleSubmit}>
    <div className="form-group">
      <label>Name of your awesome novelty</label>
      <input type="text" className="form-control" onChange={handleNameChange} value={name}></input>
      <small className="form-text text-muted">You'll identify your novelty further by it's name, so pick carefully.</small>
    </div>
    <div className="form-group">
      <label>Description</label>
      <textarea className="form-control" rows={20} onChange={handleDescriptionChange} value={description}></textarea>
    </div>
    <button type="submit" className="btn btn-primary">Add your awesome novelty!</button>
    </form>
    </LoadableContainer>
  );
  }