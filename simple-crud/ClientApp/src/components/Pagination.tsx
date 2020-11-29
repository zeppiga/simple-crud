import React, { Dispatch, SetStateAction, useState } from 'react';

export interface PaginationProps {
    pagesCount: number;
    setCurrentPageNo: Dispatch<SetStateAction<number>>;
}

export function Pagination(props: PaginationProps) {

    function onPreviousClick() {
        props.setCurrentPageNo(previous => {
            return previous === 1 ? previous : previous - 1;
        });
    }

    function onNextClick() {
        props.setCurrentPageNo(previous => {
            return previous === 1 ? previous : previous + 1;
        });
    }

    return (
        <nav className="pagination-container">
        <ul className="pagination">
          <li className="page-item"><a className="page-link" onClick={onPreviousClick}>Previous</a></li>
          {
                  [...Array(props.pagesCount+1).keys()].slice(1).map(pageNo => <li className="page-item" key={pageNo}><a className="page-link" onClick={() => props.setCurrentPageNo(pageNo)}>{pageNo}</a></li>)            
          }
          <li className="page-item"><a className="page-link" onClick={onNextClick}>Next</a></li>
        </ul>
      </nav>
    )
}