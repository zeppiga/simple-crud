import React, { useEffect, useState } from 'react';

export interface NoveltyProps {
    id : number;
    onChange: (name: string, lastChanged: Date) => void;
}

interface NoveltyDetailed {
    id: number;
    name: string;
    lastChanged: Date;
    created: Date;
    description: string;
}

export function Novelty(props: NoveltyProps) {
    const [isLoading, setIsLoading] = useState(true);
    const [novelty, setNovelty] = useState<NoveltyDetailed | null>(null);
    const [draftNovelty, setDraftNovelty] = useState<NoveltyDetailed | null>(null);
    const [isEditMode, setIsEditMode] = useState(false);

    async function loadNovelty(id: number) {
        const noveltyResponse = await fetch(`novelty/${id}`);
        const novelty: NoveltyDetailed = await noveltyResponse.json();

        setNovelty(novelty);
        setIsLoading(false);
    }

    async function updateNovelty(novelty: NoveltyDetailed) {
        setIsEditMode(false);
        setIsLoading(true);
        const body = {name: novelty.name, description: novelty.description};
        
        const noveltyRepsonse = await fetch(`novelty/${novelty.id}`, {
            method: 'PUT',
            headers: {
              'Accept': 'application/json',
              'Content-Type': 'application/json'
            },
            body: JSON.stringify(body)
        })

        const responseNovelty = await noveltyRepsonse.json();
        setNovelty(responseNovelty);
        props.onChange(responseNovelty.name, responseNovelty.lastChanged);
        setIsLoading(false);
    }

    useEffect(() => {
        loadNovelty(props.id);
    }, [props.id]);

    function onSave() {
        updateNovelty(draftNovelty!);
    }

    function onCancel() {
        setIsEditMode(false);
    }

    function onPropertyClick() {
        setIsEditMode(true);
        setDraftNovelty(novelty);
    }

    function onNameChanged(event: any) {
        const value: string = event.target.value;
        setDraftNovelty(prev => ({...prev, name: value} as NoveltyDetailed | null));
    }

    function onDescriptionChanged(event: any) {
        const value: string = event.target.value;
        setDraftNovelty(prev => ({...prev, description: value} as NoveltyDetailed | null));
    }

    if(isLoading) {
        return <div>loadin'...</div>;
    }
    else {
        return <div>
            {isEditMode ? <div className="row justify-content-end">
                    <div className="col-sm-1">
                        <input className="btn btn-primary" type="button" value="Save" onClick={onSave}></input>
                    </div>
                    <div className="col-sm-1">
                        <input className="btn btn-primary" type="button" value="Cancel" onClick={onCancel}></input>
                    </div>
                </div>: <></>}
            <div className="row">
                <div className="col-sm-12">
                <h5>{isEditMode? "Name" : "Created"}</h5>
                </div>
            </div>
            <div className="row">
                <div className="col-sm-12">
                {isEditMode ? <textarea className="form-control" rows={1} onChange={onNameChanged} value={draftNovelty!.name}></textarea>
                    : <div onClick={onPropertyClick} >{novelty!.created}</div>}
                </div>
            </div>
            <div className="row">
                <div className="col-sm-12">
                    <h5>Description</h5>
                </div>
            </div>
            <div className="row">
                <div className="col-sm-12">
                {isEditMode ? <textarea className="form-control" rows={2} onChange={onDescriptionChanged} value={draftNovelty!.description}></textarea>
                : <div onClick={onPropertyClick}>{novelty!.description}</div>}
                </div>
            </div>
        </div>
    }
}
