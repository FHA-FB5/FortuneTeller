import * as React from "react";
import {config} from "../config";

interface GroupViewProps {
  groupName: string
  personId: string
}

export class GroupView<P extends GroupViewProps> extends React.Component<P, {}> {
  render() {
    return (
      <div className="container center">
        <p className="medium">Du bist in der Gruppe</p>
        <h1 className="teal-text text-accent-4">{this.props.groupName}</h1>
        <p className="medium red-text">Um deine Gruppe später nochmal anzuschauen, speichere
          <a href={config.overviewUrl + "/person/" + this.props.personId}> diesen Link</a></p>
      </div>
    )
  }
}
