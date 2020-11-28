import React, { useState } from 'react';
import './Novelties.css';

export function Novelties() {
    const tempData = [
        {id: 1,name: "temp1", expanded: false},
        {id: 2, name: "temp2", expanded: false},
        {id: 3, name: "temp3", expanded: false} 
    ]

    const [forecasts, setForecasts] = useState(tempData);

    function onNoveltyClick(id: number) {
        setForecasts(prev => {
            const novelty = prev.find(x => x.id === id)!;

            novelty.expanded = !novelty.expanded;

            return [...prev];
        });
    }

    return (
        <div className="list-container">
            <div className="row list-header">
                <div className="col-sm-1">
                    No.
                </div>
                <div className="col-sm-2">
                    Name
                </div>
            </div>
        <div className="accordion">
            {forecasts.map((forecast, index) =>
                <div key={index}>
                    <div className="card">
                        <div className="card-header" id="headingOne">
                            <div className="list-button row" onClick={() => onNoveltyClick(forecast.id)}>
                            <div className="col-sm-1">
                                    {index}
                            </div>
                            <div className="col-sm-2">
                            {forecast.name}
                            </div>
                            </div>
                        </div>
                        <div id="collapseOne" className={forecast.expanded ? "collapse show" : "collapse"}>
                            <div className="card-body">
                                Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry richardson ad squid. 3 wolf moon officia aute, non cupidatat skateboard dolor brunch. Food truck quinoa nesciunt laborum eiusmod. Brunch 3 wolf moon tempor, sunt aliqua put a bird on it squid single-origin coffee nulla assumenda shoreditch et. Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident. Ad vegan excepteur butcher vice lomo. Leggings occaecat craft beer farm-to-table, raw denim aesthetic synth nesciunt you probably haven't heard of them accusamus labore sustainable VHS.
                  </div>
                        </div>
                    </div>
                </div>
        )}
        </div>
        </div>
      );
}