import md5 from 'md5'
import axios from 'axios'
import localStorage from '../local-storage'

var proxy = axios.create({
  withCredentials: true
})

proxy.defaults.baseURL = process.env.VUE_APP_REVERSE_PROXY_URL

proxy.interceptors.request.use(
  config => {
    const token = localStorage.chanjet.token.get()

    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }

    return config
  }
)

function getCallbackData (context: string) {
  return JSON.parse(context.split('(')[1].split(')')[0])
}

export default {
  getChanjetCode,
  getUserData,
  getHkjCode,
  getToken,
  getVmCode,
  loginVm,
  checkVmStatus,
  getBooks,
  getProjects,
  getProjectDetails
}

async function getChanjetCode () {
  const params = {
    _: 1591096872186,
    callback: 'jQuery111305083048757786359_1591166875903',
    client_id: '4cb832be-e503-4075-9903-6aa8d9e29104',
  }
  const response = await proxy.get('/internal_api/authorizeByJsonp', { params })

  if (response?.data?.result === false) {
    throw new Error(response.toString())
  } else {
    return getCallbackData(response.data).auth_code
  }
}

async function getUserData (code: string, account: string, password: string) {
  const params = {
    _: 1591096872186,
    auth_code: code,
    callback: 'jQuery111305603753566651226_1591173099393',
    auth_username: account,
    password: md5(password),
    jsonp: 1,
  }

  const response = await proxy.get('/loginV2/webLogin', { params })
  const userData = getCallbackData(response.data)

  return userData
}

async function getHkjCode () {
  const params = {
    _: 1591096872186,
    callback: 'jQuery111305603753566651226_1591173099393',
    client_id: 'accounting',
  }
  const response = await proxy.get('/internal_api/authorizeByJsonp', { params })

  if (response?.data?.result === false) {
    throw new Error(response.toString())
  } else {
    return getCallbackData(response.data).code
  }
}

async function getVmCode () {
  const params = {
    _: 1591096872186,
    callback: 'jQuery111305083048757786359_1591166875903',
    client_id: '4cb832be-e503-4075-9903-6aa8d9e29104',
  }
  const response = await proxy.get('/internal_api/authorizeByJsonp', { params })

  if (response?.data?.result === false) {
    throw new Error(response.toString())
  } else {
    return getCallbackData(response.data).code
  }
}

async function checkVmStatus (orgId: string) {
  const params = {
    appName: 'accounting',
    orgId: orgId.toString(),
    checkType: '1'
  }

  const response = await proxy.get('/vm/checkVMStatus', { params })

  return response.data
}

async function getToken (code: string) {
  const params = { code }
  const response = await proxy.get('/accounting/uzzcbkthbw2o/c0x0l6ci87/token', { params })

  return response.data
}

async function loginVm (code: string) {
  const params = { code }
  await proxy.get('/vm/login', { params })
}

async function getBooks (orgCode: string, bookCode: string) {
  const response = await proxy.get(`/accounting/${orgCode}/${bookCode}/trans/accounting/accountBook/list`)

  return response.data
}

async function getProjects (orgCode: string, bookCode: string) {
  const params = {
    page: 1,
    size: 10000,
    types: '006',
    _dc: 1591183716779
  }

  const response = await proxy.get(`accounting/${orgCode}/${bookCode}/custom/assistant/list`, {
    params
  })

  return response.data.resultObj['006']
}

async function getProjectDetails (orgCode: string, bookCode: string, code: string) {
  const queryParam = JSON.stringify({
    pageCount: 1,
    pageSize: 10000,
    subsidiaryTag: 0,
    isGlSubAccount: true,
    queryType: 'exact',
    startPeriod: '201902',
    endPeriod: '202006',
    startAssistant: {
      no: code,
      type: '006'
    },
    endAssistant: {
      no: code,
      type: '006'
    },
    displayDisableGlAccount: true,
    hideEmpty: true,
  })
  const params = { queryParam }

  const response = await proxy.get(`accounting/${orgCode}/${bookCode}/trans/gl/SubsidiaryLedge/queryPaging`, { params })

  return response.data.data.data
}
