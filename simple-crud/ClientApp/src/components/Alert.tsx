import React, { useEffect, useState } from 'react';
import "./Alert.css";

export interface AlertProps {
    message: string;
    show: boolean;
    hideAfterSeconds?: number;
    onAlertClose: () => void;
}

export function Alert(props: AlertProps) {
    const alertClassName = props.show ? visibleButtonClassName : invisibleButtonClassName;
    
    useEffect(() => {
        if (props.hideAfterSeconds && props.show) {
            setTimeout(props.onAlertClose, 1000 * props.hideAfterSeconds!)
        }
    }, [props.show]);

    return  <div className={alertClassName} role="alert">
        {props.message}
    <button type="button" className="close" data-dismiss="alert" aria-label="Close" onClick={props.onAlertClose}>
    <span aria-hidden="true">&times;</span>
  </button>
  </div>
}

const visibleButtonClassName = "alert alert-info alert-dismissible fade show";
const invisibleButtonClassName = visibleButtonClassName + " invisible";