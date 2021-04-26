function useQuery(pageSize = 15) {
  return {
    page: 1,
    pageSize,
    query: "",
  };
}

export {
  useQuery
};
