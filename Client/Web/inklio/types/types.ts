export type ODataResponse<T> = {
  context: string | null;
  nextLink: string | null;
  count: number | null;
  value: T[];
  error: any;
}

export type Ask = {
  id: Number;
  title: String;
  body: String;
}
