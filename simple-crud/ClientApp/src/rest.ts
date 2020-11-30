const defaultLowerLimitForRequestTimeInMs = 500;

export function get(url: string) {
    const requestPromise = request(url);
    return (combinePromises())(requestPromise);
}

export function post(url: string, body: any) {
    const options = {
      method: "POST",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify(body),
    };
    
    const requestPromise = request(url, options);
    return (combinePromises())(requestPromise);
}

export function put(url: string, body: any) {
  const options = {
    method: 'PUT',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(body)
  };

  const requestPromise = request(url, options);
  return (combinePromises())(requestPromise);
}

export function del(url: string) {
  const options = {
    method: 'DELETE',
    headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
    }};

    const requestPromise = request(url, options);
    return (combinePromises())(requestPromise);
}

async function request(url: string, options?: any): Promise<ResponseData> {
    const response = await fetch(url, options);
    const body = await response.json();

    return ({statusCode: response.status, contents: body});
}

function combinePromises() {
    const timePromise = new Promise(r => setTimeout(() => r(true), defaultLowerLimitForRequestTimeInMs));
    return (requestPromise: Promise<ResponseData>) => Promise.all([timePromise, requestPromise]).then(x => x[1]);
}

interface ResponseData {
    statusCode: number,
    contents: any;
}