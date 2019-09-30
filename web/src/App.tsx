import * as React from "react";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faGithub} from "@fortawesome/free-brands-svg-icons";
import {faStar} from "@fortawesome/free-solid-svg-icons";
import Wave from "./components/wave/Wave";
import {RegisterView} from "./views/RegisterView";
import "./config"
import {config} from "./config";
import "./App.css"
import {Group} from "./api/model";
import {GroupView} from "./views/GroupView";

interface AppState {
  isRegistered: boolean
  registrationFailed: boolean
  groupName: string
  personID: string
}

class App<T extends AppState> extends React.Component<{}, T> {
  constructor() {
    super({})
    this.state = {
      isRegistered: false
    } as T
  }

  render() {
    return (
      <div className="App teal accent-4" style={{minHeight: "100vh"}}>
        <div className="row teal accent-4" style={{height: "12em", margin: 0}}>
          <div className="col s12">
            <h1 className="show-on-large white-text center"
                style={{display: "none", whiteSpace: "nowrap", fontFamily: "'Parisienne'", fontSize: "5em"}}>
              {config.projectName}
            </h1>
            <h1 className="show-on-medium-and-down white-text center"
                style={{display: "none", whiteSpace: "nowrap", fontFamily: "'Parisienne'", fontSize: "3em"}}>
              Fortune Teller
            </h1>
          </div>
        </div>
        <div className="body white row valign-wrapper">
          {this.state.isRegistered
            ?
            <GroupView groupName={this.state.groupName} personId={this.state.personID}/>
            :
            <RegisterView onFailure={() => this.setState({registrationFailed: true})} onSuccess={response => {
              fetch(config.apiUrl + "/group/" + response.groupId, {
                method: 'get'
              }).then(response => {
                if (response.status !== 200) {
                  throw Error(`invalid status: ${response.status} ${response.statusText}`)
                }
                return response.json()
              }).then(group => {
                console.log((group as Group).id)
                this.setState({
                  isRegistered: true,
                  groupName: (group as Group).name,
                  personID: response.id
                })
              })
            }}/>
          }
        </div>
        <footer className="row page-footer teal accent-4" style={{height: "3em", margin: 0}}>
          <div className="footer-copyright">
            <div className="container">
              © 2019 <a href={config.copyrightUrl} className="grey-text text-lighten-2">{config.copyrightOwnerShort}</a>
              <div className="right">
                <a href={config.sourceUrl} className="grey-text text-lighten-2">
                  <FontAwesomeIcon icon={faStar}/> me on <FontAwesomeIcon icon={faGithub}/>
                </a>
              </div>
            </div>
          </div>
        </footer>
      </div>
    )
  }
}

export default App;
