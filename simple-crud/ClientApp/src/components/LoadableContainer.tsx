import React from 'react';
import "./LoadableContainer.css";

export function LoadableContainer(props: {isLoading: boolean, children: any}) {
    const className = !props.isLoading ? "loadable-container" : "loadable-container-active";
    
    return <>
        {props.isLoading ? <div className="spinner-grow" role="status">
            <span className="sr-only">Loading...</span>
        </div> : <></>}
        <div className={className}>
            {props.children}
        </div>
    </>
}