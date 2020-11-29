import React, { useEffect, useState } from 'react';

export interface NoveltyProps {
    id : number;
}

interface NoveltyDetailed {
    id: number;
    name: string;
    lastChanged: Date;
    created: Date;
    description: string;
}

export function Novelty(props: NoveltyProps) {
    async function loadNovelty(id: number) {
        const noveltyResponse = await fetch(`novelty/${id}`);
        const novelty: NoveltyDetailed = await noveltyResponse.json();

        setNovelty(novelty);
        setIsLoading(false);
    }

    const [isLoading, setIsLoading] = useState(true);
    const [novelty, setNovelty] = useState<NoveltyDetailed | null>(null)

    useEffect(() => {
        loadNovelty(props.id);
    }, [props.id]);

    if(isLoading) {
        return <div>loadin'...</div>;
    }
    else {
        return <div>
            <h5>Created</h5>
            <div>{novelty!.created}</div>
            <h5>Description</h5>
            <div>{novelty!.description}</div>
            </div>
    }
}
