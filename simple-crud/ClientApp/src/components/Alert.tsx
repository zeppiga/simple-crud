import React, { useEffect } from 'react';
import "./Alert.css";

export interface AlertProps {
    alertType: AlertType;
    message: string;
    show: boolean;
    doNotHide?: number;
    onAlertClose: () => void;
}

const defaultHideTimeInSeconds = 10;

export function Alert(props: AlertProps) {
    const alertClassName = getAlerClassName(props.alertType, props.show);
    
    useEffect(() => {
        if (!props.doNotHide && props.show) {
            const timeout = setTimeout(props.onAlertClose, 1000 * defaultHideTimeInSeconds!)

            return () => clearTimeout(timeout);
        }
    }, [props.show, props.doNotHide, props.onAlertClose]);

    return  <div className={alertClassName} role="alert">
        {props.message}
    <button type="button" className="close" data-dismiss="alert" aria-label="Close" onClick={props.onAlertClose}>
    <span aria-hidden="true">&times;</span>
  </button>
  </div>
}

export enum AlertType {
    Info,
    Error
}

function getAlerClassName(type: AlertType, visible: boolean) {
    if (!visible) {
        return "alert fade show invisible";
    }

    switch(type) {
        case AlertType.Info:
            return "alert alert-info alert-dismissible fade show";
        case AlertType.Error:
            return "alert alert-danger alert-dismissible fade show";
    }
}