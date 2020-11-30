import React, { Dispatch, SetStateAction, useState } from 'react';

export interface PaginationProps {
    pagesCount: number;
    setCurrentPageNo: Dispatch<SetStateAction<number>>;
}

export function Pagination(props: PaginationProps) {
    const [currentPageNo, setCurrentPageNo] = useState(1);

    function onPreviousClick() {
        props.setCurrentPageNo(previous => {
            const toSet = previous === 1 ? previous : previous - 1;
            setCurrentPageNo(toSet);
            return toSet;
        });
    }

    function onNextClick() {
        props.setCurrentPageNo(previous => {
            const toSet = previous === props.pagesCount ? previous : previous + 1;
            setCurrentPageNo(toSet);
            return toSet;
        });
    }

    function getButtonClassName(buttonNumber: number) {
        return buttonNumber === currentPageNo ? "page-item active" :"page-item";  
    }

    function onButtonClick(pageNo: number) {
        setCurrentPageNo(pageNo);
        props.setCurrentPageNo(pageNo)
    }

    // TODO keyboard navigation is broken
    return (
        <nav className="pagination-container">
        <ul className="pagination">
          <li className="page-item"><a className="page-link" onClick={onPreviousClick}>Previous</a></li>
          {
                  [...Array(props.pagesCount+1).keys()].slice(1).map(pageNo => <li className={getButtonClassName(pageNo)} key={pageNo}><a className="page-link" onClick={() => onButtonClick(pageNo)}>{pageNo}</a></li>)            
          }
          <li className="page-item"><a className="page-link" onClick={onNextClick}>Next</a></li>
        </ul>
      </nav>
    )
}